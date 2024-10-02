using DoAnWeb.ThanhToan;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DoAnWeb.Pages.PageBanVe
{
    public class ThanhToanModel : PageModel
    {
        public int TicketPrice { get; set; }
        public Service _service { get; set; }
        public ThanhToanModel(Service service)
        {
            _service = service;
        }
        public void OnGet()
        {
            PaymentInformationModel.Amount = _service.GiaVeTT;
            Console.WriteLine(_service.GheDangChon);
        }
    }
}
