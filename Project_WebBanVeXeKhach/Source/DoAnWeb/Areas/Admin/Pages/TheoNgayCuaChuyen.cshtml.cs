using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace DoAnWeb.Areas.Admin.Pages.DoanhThu
{
    public class TheoNgayCuaChuyenModel : PageModel
    {
        public List<thongke> ThongKe { get; set; }

        public void OnGet()
        {
            ThongKe = new List<thongke>();

            using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
            {
                connection.Open();
                string storedProcedureName = "ThongKeTheoNgayCuaChuyen";
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            thongke tk = new thongke
                            {
                                ThoiGianSP = reader.GetDateTime(0),
                                MaChuyenXe = reader.GetString(1),
                                MaTuyen = reader.GetString(2),
                                SoVeDaBan = reader.GetInt32(3),
                                DoanhThu = reader.GetInt32(4),
                            };
                            ThongKe.Add(tk);
                        }
                    }
                }
            }
        }

        public class thongke
        {
            public string MaChuyenXe { get; set; }
            public string MaTuyen { get; set; }
            public int DoanhThu { get; set; }
            public DateTime ThoiGianSP { get; set; }
            public int SoVeDaBan { get; set; }
        }
    }
}
