using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace DoAnWeb.Areas.Admin.Pages
{
    public class CEGiaVeModel : PageModel
    {
        public List<string> MaTuyenList { get; set; }
        public string Chucnang { get; set; }
        [BindProperty]
        public string matuyenxe { get; set; }
        [BindProperty]
        public int GiaVe { get; set; }
        [BindProperty]
        public DateTime NgayApDung { get; set; }
        public IActionResult OnGet(string maTuyenXe, string chucnang)
        {
            matuyenxe = maTuyenXe;
            Chucnang = chucnang;
            GetLstTuyen();
            return Page();
        }
        public IActionResult OnPostEdit()
        {
            UpdateGV();
            return RedirectToPage("/QLGiaVe");
        }
        public IActionResult OnPostCreate()
        {
            CreateGV();
            return RedirectToPage("/QLGiaVe");
        }
        public void CreateGV()
        {
            using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
            {
                connection.Open();
                string insertQuery = $"INSERT INTO QLGiaVe (MaTuyen, GiaVe, NgayApDung) VALUES ('{matuyenxe}','{GiaVe}', '{NgayApDung}')";
                Console.WriteLine(insertQuery);
                // Thực hiện truy vấn thêm chuyến xe
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.ExecuteNonQuery(); // Thực thi câu lệnh SQL để thêm dữ liệu
                }
            }
        }
        public void UpdateGV()
        {
            using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
            {
                connection.Open();
                string updateQuery = $@"UPDATE QLGiaVe SET GiaVe = '{GiaVe}' WHERE MaTuyen = '{matuyenxe}' AND NgayApDung = '{NgayApDung}' ";
                Console.WriteLine(updateQuery);
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    int rowsAffected = command.ExecuteNonQuery(); 
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
    }
}
