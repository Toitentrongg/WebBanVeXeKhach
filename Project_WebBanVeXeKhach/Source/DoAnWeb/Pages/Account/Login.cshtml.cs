using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using static DoAnWeb.Service;

namespace DoAnWeb.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential credential { get; set; }

        public void OnGet()
        {

        }
        public void Checklogin() 
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
            {
                connection.Open();

                // Truy vấn SQL để kiểm tra thông tin đăng nhập
                string query = "SELECT QuyenHan FROM Users WHERE TenDangNhap = @TenDangNhap AND MatKhau = @MatKhau";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TenDangNhap", credential.UserName);
                    command.Parameters.AddWithValue("@MatKhau", credential.Password);

                    // Thực hiện truy vấn và lấy giá trị quyền hạn từ cơ sở dữ liệu
                    object roleObject = command.ExecuteScalar();

                    if (roleObject != null)
                    {
                        string role = roleObject.ToString();
                        List<Claim> lst = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, credential.UserName),
                            new Claim(ClaimTypes.Name, credential.UserName),
                            new Claim(ClaimTypes.Role, role),
                        };
                        ClaimsIdentity ci = new ClaimsIdentity(lst,
                            Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme
                            );
                        ClaimsPrincipal cp = new ClaimsPrincipal(ci);
                        await HttpContext.SignInAsync(cp);
                        if (role != "QuanTri")
                        {
                            return RedirectToPage("/Home");
                        }
                        else
                        {
                            return Redirect("/Admin/Homeadmin");
                        }
                    }
                    else
                    {
                        return RedirectToPage("/account/login");
                    }
                }
            }
        }
        public class Credential
        {
            [Required]
            public string UserName { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}
