using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;

namespace DoAnWeb.Areas.Admin.Pages
{
    public class CEUsersModel : PageModel
    {
        public Service _service { get; set; }
        public CEUsersModel(Service service)
        {
            _service = service;
        }
        public string chucnang { get; set; }
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
        public IActionResult OnGet(string TenDangNhap, string chucnang)
        {
            this.TenDangNhap = TenDangNhap;
            this.chucnang = chucnang;
            return Page();
        }
        public IActionResult OnPostEdit()
        {
            UpdateUser();
            return RedirectToPage("/QLChuyenXe");
        }
        public IActionResult OnPostCreate()
        {
            CreateUser();
            return RedirectToPage("/QLChuyenXe");
        }
        public void CreateUser()
        {
            using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
            {
                connection.Open();
                string insertQuery = $"INSERT INTO Users (TenDangNhap, MatKhau, QuyenHan, TenUser, DiaChi, SoDT, SoVeMua) " +
                                     $"VALUES (@TenDangNhap, @MatKhau, @QuyenHan, @TenUser, @DiaChi, @SoDT, @SoVeMua)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    // Thêm các tham số và giá trị tương ứng vào câu truy vấn
                    command.Parameters.AddWithValue("@TenDangNhap", TenDangNhap);
                    command.Parameters.AddWithValue("@MatKhau", MatKhau);
                    command.Parameters.AddWithValue("@QuyenHan", QuyenHan);
                    command.Parameters.AddWithValue("@TenUser", TenUser);
                    command.Parameters.AddWithValue("@DiaChi", DiaChi);
                    command.Parameters.AddWithValue("@SoDT", SoDT);
                    command.Parameters.AddWithValue("@SoVeMua", SoVeMua);

                    // Thực thi câu truy vấn
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateUser()
        {
            using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
            {
                connection.Open();
                string updateQuery = $@"UPDATE Users SET MatKhau = '{MatKhau}', QuyenHan = '{QuyenHan}', TenUser = '{TenUser}',DiaChi = '{DiaChi}',SoDT = '{SoDT}',SoVeMua = {SoVeMua} WHERE TenDangNhap = '{TenDangNhap}'";
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
