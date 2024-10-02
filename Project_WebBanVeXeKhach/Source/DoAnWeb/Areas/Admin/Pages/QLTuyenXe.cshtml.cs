using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace DoAnWeb.Areas.Admin.Pages
{
    public class QLTuyenXeModel : PageModel
    {
        public List<TuyenXe> DanhSachTuyenXe { get; set; }
        public string maTuyen;

        public void OnGet()
        {
            maTuyen = "";
            maTuyen = Request.Query["maTuyenXe"];
            if (maTuyen != "")
            {
                using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
                {
                    connection.Open();
                    string deleteQuery = $"DELETE FROM TuyenXe WHERE MaTuyen = '{maTuyen}'";

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
            DanhSachTuyenXe = new List<TuyenXe>();

            using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
            {
                connection.Open();
                string query = "SELECT MaTuyen, DiemBatDau, DiemKetThuc FROM TuyenXe";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TuyenXe tuyenxe = new TuyenXe
                            {
                                MaTuyenXe = reader.GetString(0),
                                DiemBatDau = reader.GetString(1),
                                DiemKetThuc = reader.GetString(2),
                            };
                            DanhSachTuyenXe.Add(tuyenxe);
                        }
                    }
                }
            }
        }
        public class TuyenXe
        {
            public string MaTuyenXe { get; set; }
            public string DiemBatDau { get; set; }
            public string DiemKetThuc { get; set; }
        }
    }
}
