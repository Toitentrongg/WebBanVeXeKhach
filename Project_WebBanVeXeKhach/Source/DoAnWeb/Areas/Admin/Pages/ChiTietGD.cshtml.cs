using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace DoAnWeb.Areas.Admin.Pages
{
    public class ChiTietGDModel : PageModel
    {
        public DataTable ChiTietGD { get; set; }

        public void OnGet()
        {
            using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
            {
                connection.Open();
                string Query = "SELECT * FROM ThanhToan";
                using (SqlConnection con = new SqlConnection(SQLConnect.Conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(Query, con))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        ChiTietGD = new DataTable();
                        adapter.Fill(ChiTietGD);
                    }
                }
            }
        }
    }
}
