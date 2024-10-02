using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace DoAnWeb.Areas.Admin.Pages.DoanhThu
{
    public class TheoTuyenModel : PageModel
    {
        public List<thongke> ThongKe { get; set; }

        public void OnGet()
        {
            ThongKe = new List<thongke>();

            using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
            {
                connection.Open();
                string storedProcedureName = "GetDoanhThuTheoTuyen";
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            thongke tk = new thongke
                            {
                                DiemBatDau = reader.GetString(0),
                                DiemKetThuc = reader.GetString(1),
                                Thang = reader.GetInt32(2),
                                Nam = reader.GetInt32(3),
                                SoVeDaBan = reader.GetInt32(4),
                                DoanhThu = reader.GetInt32(5),
                            };
                            ThongKe.Add(tk);
                        }
                    }
                }
            }
        }
        public class thongke
        {
            public string DiemBatDau { get; set; }
            public string DiemKetThuc { get; set; }
            public int Thang { get; set; }
            public int Nam { get; set; }

            public int SoVeDaBan { get; set; }
            public int DoanhThu { get; set; }
        }
    }
}
