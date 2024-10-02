using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static DoAnWeb.Areas.Admin.Pages.QLTuyenXeModel;
using System.Data.SqlClient;

namespace DoAnWeb.Areas.Admin.Pages
{
    public class QLGIaVeModel : PageModel
    {
        public List<GiaVe> DanhSachGiaVe { get; set; }
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
                    string deleteQuery = $"DELETE FROM QLGiaVe WHERE MaTuyen = '{maTuyen}'";

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
            DanhSachGiaVe = new List<GiaVe>();

            using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
            {
                connection.Open();
                string query = "SELECT MaTuyen, GiaVe, NgayApDung FROM QLGiaVe";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            GiaVe giave = new GiaVe
                            {
                                MaTuyenXe = reader.GetString(0),
                                giaVe = reader.GetInt32(1),
                                NgayApDung = reader.GetDateTime(2),
                            };
                            DanhSachGiaVe.Add(giave);
                        }
                    }
                }
            }
        }
    }
    public class GiaVe
    {
        public string MaTuyenXe { get; set; }
        public int giaVe { get; set; }
        public DateTime NgayApDung { get; set; }
    }
}
