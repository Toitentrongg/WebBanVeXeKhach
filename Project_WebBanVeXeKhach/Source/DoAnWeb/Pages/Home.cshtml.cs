using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DoAnWeb.Pages
{

    public class HomeModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Start { get; set; }

        [BindProperty(SupportsGet = true)]
        public string End { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime departure { get; set; }
        public Service _service { get; set; }
        public HomeModel(Service service)
        {
            _service = service;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            _service.Start = Start;
            _service.End = End;
            _service.Departure = departure.ToString("yyyy-MM-dd");

            // Ở đây bạn có thể thực hiện các xử lý khi form được submit
            // Dữ liệu sẽ được gửi đến server qua phương thức POST
            return RedirectToPage("/PageBanVe/PageBanVe");
        }
    }
}
