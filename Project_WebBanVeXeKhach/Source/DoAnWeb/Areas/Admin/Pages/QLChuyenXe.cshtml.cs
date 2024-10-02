using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace DoAnWeb.Areas.Admin.Pages
{
    public class QLChuyenXeModel : PageModel
    {
        public List<ChuyenXe> DanhSachChuyenXe { get; set; }
        public string maCX;
        public void OnGet()
        {
            maCX = "";
            maCX = Request.Query["maChuyenXe"];
            if(maCX!="")
            {
                using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
                {
                    connection.Open(); 
                    string deleteQuery = $"DELETE FROM ChuyenXe WHERE MaChuyenXe = '{maCX}'";

                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
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
            DanhSachChuyenXe = new List<ChuyenXe>();

            using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
            {
                connection.Open();
                string query = "SELECT MaChuyenXe, MaTuyen, ThoiGianXuatPhat, SoLuongGhe, SoVeCon FROM ChuyenXe";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ChuyenXe chuyenXe = new ChuyenXe
                            {
                                MaChuyenXe = reader.GetString(0),
                                MaTuyen = reader.GetString(1),
                                ThoiGianXuatPhat = reader.GetDateTime(2),
                                SoLuongGhe = reader.GetInt32(3), // Chỉ mục 3 là SoLuongGhe
                                SoVeCon = reader.GetInt32(4)     // Chỉ mục 4 là SoVeCon
                            };
                            DanhSachChuyenXe.Add(chuyenXe);
                        }
                    }
                }
            }
        }
        public void OnPost()
        {
        }
    }
    public class ChuyenXe
    {
        public string MaChuyenXe { get; set; }
        public string MaTuyen { get; set; }
        public DateTime ThoiGianXuatPhat { get; set; }
        public TimeSpan ThoiGianDi { get; set; }
        public int SoLuongGhe { get; set; }
        public int SoVeCon { get; set; }
    }
}
