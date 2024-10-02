using DoAnWeb.ThanhToan;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DoAnWeb.Pages.Home
{
    public class ThanhToanThanhCongModel : PageModel
    {
        public string ResponseCode { get; set; }

        public Service _service { get; set; }
        public ThanhToanThanhCongModel(Service service)
        {
            _service = service;
        }
        public void OnGet([FromQuery(Name = "responseCode")] string responseCode)
        {
            ResponseCode = responseCode;
            if (ResponseCode == "00")
            {
                string[] numberStrings = _service.GheDangChon.Split(',');
                foreach (string numberStr in numberStrings)
                {
                    if (int.TryParse(numberStr.Trim(), out int number))
                    {
                        using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
                        {
                            connection.Open();
                            string updateQuery = $"UPDATE VeXe SET TenDangNhap = '{User.Identity.Name}' , NgayMua = '{DateTime.Now.ToString("yyyy-MM-dd")}', TrangThai = 1 WHERE MaChuyenXe = '{_service.MaChuyen}' AND ThoiGianXuatPhat = '{_service.Departure}' AND SoGhe = {number};";
                            using (SqlCommand cmdUpdate = new SqlCommand(updateQuery, connection))
                            {

                                Console.WriteLine(updateQuery);
                                cmdUpdate.ExecuteNonQuery();
                            }
                            _service.GetMaVeXe(number);
                            string UpdateTTQuery = $"INSERT INTO ThanhToan(MaVeXe , TenDangNhap , ThoiGianGiaoDich,TongTien,TrangThai) VALUES ('{_service.maVeXe}','{User.Identity.Name}','{DateTime.Now.ToString("yyyy-MM-dd")}', '{_service.GiaVeTT}','Thanh Cong');";
                            using (SqlCommand cmdUpdate = new SqlCommand(UpdateTTQuery, connection))
                            {
                                Console.WriteLine(UpdateTTQuery);
                                cmdUpdate.ExecuteNonQuery();
                            }
                        }


                    }
                }
            }
            else
            {
                string[] numberStrings = _service.GheDangChon.Split(',');
                foreach (string numberStr in numberStrings)
                {
                    if (int.TryParse(numberStr.Trim(), out int number))
                    {
                        using (SqlConnection connection = new SqlConnection(SQLConnect.Conn))
                        {
                            connection.Open();
                            _service.GetMaVeXe(number);
                            string UpdateTTQuery = $"INSERT INTO ThanhToan(MaVeXe , TenDangNhap , ThoiGianGiaoDich,TongTien,TrangThai) VALUES ('{_service.maVeXe}','{User.Identity.Name}','{DateTime.Now.ToString("yyyy-MM-dd")}', '{_service.GiaVeTT}','That Bai');";
                            using (SqlCommand cmdUpdate = new SqlCommand(UpdateTTQuery, connection))
                            {
                                cmdUpdate.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }
    }
}
