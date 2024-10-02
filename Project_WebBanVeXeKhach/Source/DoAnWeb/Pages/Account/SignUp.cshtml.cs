using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace DoAnWeb.Pages.Account
{
    public class SignUpModel : PageModel
    {
        private readonly ILogger<SignUpModel> _logger;
        public SignUpModel(ILogger<SignUpModel> logger)
        {
            _logger = logger;
        }
        [BindProperty]
        public string tendangnhap { get; set; }
        [BindProperty]
        public string matkhau { get; set; }
        public void OnGet()
        {
            TempData["Message"] = "";
        }
        public IActionResult OnPost()
        {
            if (!CheckAccountExists(tendangnhap))
            {
                // Thêm tài khoản mới
                InsertUser(tendangnhap, matkhau);
                TempData["Message"] = "Thêm tài khoản thành công.";
                return RedirectToPage("Login");
            }
            else
            {
                TempData["Message"] = "Tài khoản đã tồn tại.";
                return Page();
            }

        }
        public void InsertUser(string tendangnhap, string matkhau)
        {
            using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
            {
                connection.Open();
                string query = "INSERT INTO Users (TenDangNhap, MatKhau, QuyenHan,TenUser,DiaChi,SoDT,SoVeMua) " +
                               "VALUES (@TenDangNhap, @MatKhau, @QuyenHan,'','','',0)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TenDangNhap", tendangnhap);
                    command.Parameters.AddWithValue("@MatKhau", matkhau);
                    command.Parameters.AddWithValue("@QuyenHan", "KhachHang");

                    command.ExecuteNonQuery(); // Thực thi truy vấn để thêm người dùng mới
                }
            }
        }

        private bool CheckAccountExists(string username)
        {
            using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
            {
                connection.Open();
                string query = $"SELECT COUNT(*) FROM Users WHERE TenDangNhap = @Username";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}
