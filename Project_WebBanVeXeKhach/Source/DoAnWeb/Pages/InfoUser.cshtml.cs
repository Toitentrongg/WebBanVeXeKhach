using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static DoAnWeb.Areas.Admin.Pages.QLUsersModel;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel.DataAnnotations;
using static DoAnWeb.Pages.LoginModel;

namespace DoAnWeb.Pages
{
    public class InfoUserModel : PageModel
    {
        [BindProperty]
        public Users userr { get; set; }
        public string Chucnang {  get; set; }

        public IActionResult OnGet(string chucnang)
        {
            Chucnang= chucnang;
            Console.WriteLine(Chucnang);
            using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
            {
                connection.Open();
                string query = $"SELECT TenDangNhap,MatKhau,QuyenHan,TenUser,DiaChi,SoDT,SoVeMua FROM Users WHERE TenDangNhap = '{User.Identity.Name}'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userr = new Users
                            {
                                TenDangNhap = reader.GetString(0),
                                MatKhau = reader.GetString(1),
                                QuyenHan = reader.GetString(2),
                                TenUser = reader.GetString(3),
                                DiaChi = reader.GetString(4),
                                SoDT = reader.GetString(5),
                                SoVeMua = reader.GetInt32(6),
                            };
                        }
                    }
                }
            }
            return Page();
        }
        public void UpdateUser()
        {
            using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
            {
                connection.Open();
                string updateQuery = $@"UPDATE Users SET MatKhau = '{userr.MatKhau}', TenUser = '{userr.TenUser}',DiaChi = '{userr.DiaChi}',SoDT = '{userr.SoDT}' WHERE TenDangNhap = '{User.Identity.Name}'";
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
        public IActionResult OnPost()
        {
            UpdateUser();
            return RedirectToPage("/InfoUser");
        }
        public class Users
        {
            [BindProperty]
            public string TenDangNhap { get; set; }
            [BindProperty]
            public string MatKhau { get; set; }
            [BindProperty]
            public string QuyenHan { get; set; }
            [BindProperty]

            public string TenUser { get; set; }
            [BindProperty]
            public string DiaChi { get; set; }
            [BindProperty]
            public string SoDT { get; set; }
            [BindProperty]
            public int SoVeMua { get; set; }
        }
    }
}
