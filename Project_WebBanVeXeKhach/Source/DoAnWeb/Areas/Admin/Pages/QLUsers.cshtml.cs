using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace DoAnWeb.Areas.Admin.Pages
{
    public class QLUsersModel : PageModel
    {
        public List<Users> User { get; set; }
        public string tenDN;

        public void OnGet()
        {
            tenDN = "";
            tenDN = Request.Query["TenDangNhap"];
            if (tenDN != "")
            {
                using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
                {
                    connection.Open();
                    string deleteQuery = $"DELETE FROM Users WHERE TenDangNhap = '{tenDN}'";

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
            User = new List<Users>();

            using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
            {
                connection.Open();
                string query = "SELECT TenDangNhap,MatKhau,QuyenHan,TenUser,DiaChi,SoDT,SoVeMua FROM Users";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Users user = new Users
                            {
                                TenDangNhap = reader.GetString(0),
                                MatKhau = reader.GetString(1),
                                QuyenHan = reader.GetString(2),
                                TenUser = reader.GetString(3),
                                DiaChi = reader.GetString(4),
                                SoDT = reader.GetString(5),
                                SoVeMua = reader.GetInt32(6),
                            };
                            User.Add(user);
                        }
                    }
                }
            }
        }
        public class Users
        {
            public string TenDangNhap { get; set; }
            public string MatKhau { get; set; }
            public string QuyenHan { get; set; }
            public string TenUser { get; set; }
            public string DiaChi { get; set; }
            public string SoDT { get; set; }
            public int SoVeMua { get; set; }
        }
    }
}
