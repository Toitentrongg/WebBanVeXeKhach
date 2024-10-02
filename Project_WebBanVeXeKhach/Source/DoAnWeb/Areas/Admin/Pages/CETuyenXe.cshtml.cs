using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;

namespace DoAnWeb.Areas.Admin.Pages
{
    public class CETuyenXeModel : PageModel
    {
        public string Chucnang { get; set; }
        [BindProperty]
        public string matuyenxe { get; set; }
        [BindProperty]
        public string NoiDi {  get; set; }
        [BindProperty]
        public string NoiDen {  get; set; }
        public IActionResult OnGet(string maTuyenXe, string chucnang)
        {
            matuyenxe = maTuyenXe;
            Chucnang = chucnang;
            return Page();
        }
        public IActionResult OnPostEdit()
        {
            UpdateTX();
            return RedirectToPage("/QLTuyenXe");
        }
        public IActionResult OnPostCreate()
        {
            CreateTX();
            return RedirectToPage("/QLTuyenXe");
        }
        public void CreateTX()
        {
            using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
            {
                connection.Open();
                string insertQuery = $"INSERT INTO TuyenXe (MaTuyen, DiemBatDau, DiemKetThuc) VALUES ('{matuyenxe}','{NoiDi}', '{NoiDen}')";
                Console.WriteLine(insertQuery);
                // Thực hiện truy vấn thêm chuyến xe
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.ExecuteNonQuery(); // Thực thi câu lệnh SQL để thêm dữ liệu
                }
            }
        }

        public void UpdateTX()
        {
            using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
            {
                connection.Open();
                string updateQuery = $@"UPDATE TuyenXe SET DiemBatDau = '{NoiDi}', DiemKetThuc = '{NoiDen}'  WHERE MaTuyen = '{matuyenxe}'";
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
