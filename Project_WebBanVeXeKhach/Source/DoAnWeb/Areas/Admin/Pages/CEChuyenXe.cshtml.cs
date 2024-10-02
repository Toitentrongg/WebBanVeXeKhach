using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DoAnWeb.Areas.Admin.Pages
{
    public class EditChuyenXeModel : PageModel
    {
        public Service _service { get; set; }
        public EditChuyenXeModel(Service service)
        {
            _service = service;
        }
        public List<string> MaTuyenList { get; set; }
        [BindProperty]
        public string maCX { get; set; }
        [BindProperty]
        public string maTuyen { get; set; }
        [BindProperty]
        public DateTime TGianXP { get; set; }
        [BindProperty]
        public int SLGhe { get; set; }
        [BindProperty]
        public int SoVe { get; set; }
        [BindProperty]
        public string Chucnang { get; set; }
        public IActionResult OnGet(string maChuyenXe,string chucnang)
        {
            maCX = maChuyenXe;
            Chucnang = chucnang;
            GetLstTuyen();
            return Page();
        }

        public void GetLstTuyen()
        {
            MaTuyenList = new List<string>();

            // Kết nối đến cơ sở dữ liệu
            using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
            {
                connection.Open();
                string query = "SELECT MaTuyen FROM TuyenXe";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Đọc từng dòng kết quả và thêm mã tuyến vào danh sách
                        while (reader.Read())
                        {
                            MaTuyenList.Add(reader.GetString(0));
                        }
                    }
                }
            }
        }

        public IActionResult OnPostEdit()
        {
            UpdateCX();
            return RedirectToPage("/QLChuyenXe");
        }
        public IActionResult OnPostCreate()
        {
            CreateCX();
            return RedirectToPage("/QLChuyenXe");
        }
        public void CreateCX()
        {
            using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
            {
                connection.Open();
                string insertQuery = $"INSERT INTO ChuyenXe (MaChuyenXe, MaTuyen, ThoiGianXuatPhat, SoLuongGhe, SoVeCon) VALUES ('{maCX}', '{maTuyen}', '{TGianXP}', '{SLGhe}', '{SoVe}')";
                Console.WriteLine(insertQuery);

                // Thực hiện truy vấn thêm chuyến xe
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Inserted {rowsAffected} rows");

                        // Gọi store procedure để tạo vé xe
                        string procedureName = "InsertVeXe";
                        using (SqlCommand procedureCommand = new SqlCommand(procedureName, connection))
                        {
                            procedureCommand.CommandType = CommandType.StoredProcedure;
                            procedureCommand.Parameters.AddWithValue("@MaCX", maCX);
                            procedureCommand.Parameters.AddWithValue("@ThoiGianXuatPhat",TGianXP);
                            procedureCommand.Parameters.AddWithValue("@SLGhe", SLGhe);

                            int rowsAffectedProcedure = procedureCommand.ExecuteNonQuery();
                            if (rowsAffectedProcedure > 0)
                            {
                                Console.WriteLine($"Store procedure executed successfully. Rows affected: {rowsAffectedProcedure}");
                            }
                            else
                            {
                                Console.WriteLine("Store procedure execution failed or no rows affected.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No rows were affected.");
                    }
                }
            }
        }
        public void UpdateCX()
        {
            using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
            {
                connection.Open();
                string updateQuery = $@"UPDATE ChuyenXe SET MaTuyen = '{maTuyen}', ThoiGianXuatPhat = '{TGianXP}', SoLuongGhe = '{SLGhe}', SoVeCon = '{SoVe}' WHERE MaChuyenXe = '{maCX}'";
                Console.WriteLine(updateQuery);
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    int rowsAffected = command.ExecuteNonQuery(); // Thực thi câu lệnh SQL và lấy số hàng bị ảnh hưởng
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Deleted {rowsAffected} rows");
                    }
                    else
                    {
                        Console.WriteLine("No rows were affected.");
                    }
                }
            }
        }
    }
}
