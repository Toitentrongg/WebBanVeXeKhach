using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;

namespace DoAnWeb.Pages.PageBanVe
{
    public class PageBanVeModel : PageModel
    {
        public DataTable ChuyenXe { get; set; }

        public Service _service { get; set; }
        public PageBanVeModel(Service service)
        {
            _service = service;
        }
        public void TimKiem1(string query)
        {
            using (SqlConnection con = new SqlConnection(SQLConnect.Conn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    ChuyenXe = new DataTable();
                    adapter.Fill(ChuyenXe);
                    Console.WriteLine(query);
                }
            }
        }
        public void OnGet()
        {
            string query = $"SELECT * FROM ChuyenXe INNER JOIN TuyenXe ON ChuyenXe.MaTuyen = TuyenXe.MaTuyen WHERE DiemBatDau = N'{_service.Start}' AND DiemKetThuc = N'{_service.End}' and ThoiGianXuatPhat = '{_service.Departure}'";
            TimKiem1(query);
            
        }
        public IActionResult OnPost()
        {
            //_service.MaChuyen = maChuyenXe;
            Console.WriteLine(_service.MaChuyen);
            return RedirectToPage("/PageBanVe/ChonGhe");
        }
    }
}
