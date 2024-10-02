using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;

namespace DoAnWeb.Pages.PageBanVe
{
    public class ChonGheModel : PageModel
    {
        public Service _service { get; set; }
        public ChonGheModel(Service service)
        {
            _service = service;
        }
        DateTime ngayHienTai = DateTime.Now;
        string maCX { get; set; }
        [BindProperty]
        public int ticketPriceInput { get; set; }
        [BindProperty]
        public string GheDangChon { get; set; }
        public void OnGet()
        {
            _service.lstxedaban.Clear();
            // Lấy giá trị của tham số "maChuyenXe" từ URL parameters
            maCX = Request.Query["maChuyenXe"];
            _service.MaChuyen = maCX;
            _service.GetSLGhe();
            for (int i = 1; i <=_service.SLGhe; i++) 
            {
                _service.LayTrangThai(i);
            }
            foreach(var i in _service.lstxedaban)
            {
                Console.WriteLine(i);
            }
            _service.GetmaTuyen();
            _service.GetGiaVe(ngayHienTai);
        }
        public IActionResult OnPost()
        {
            _service.GiaVeTT = ticketPriceInput;
            _service.GheDangChon = GheDangChon;
            return RedirectToPage("/PageBanVe/ThanhToan");
        }
    }
}
