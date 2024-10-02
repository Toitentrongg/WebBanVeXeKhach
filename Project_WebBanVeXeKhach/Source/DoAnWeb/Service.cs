using System.Data.SqlClient;

namespace DoAnWeb
{
    public class Service
    {
        public string Start { get; set; }
        public string End { get; set; }
        public string Departure { get; set; }
        public string MaChuyen { get; set; }
        public int SLGhe {  get; set; }
        public string maTuyen { get; set; }
        public int GiaVeTT { get; set; }
        public string GheDangChon { get; set; }
        public string maVeXe {  get; set; }
        public int GiaVe { get; set; }
        public List<bool> lstxedaban = new List<bool>();
        public void LayTrangThai(int sott)
        {
            int trangThai = -1;
            using (SqlConnection con = new SqlConnection(SQLConnect.Conn))
            {
                con.Open();

                string query = @"SELECT TrangThai FROM VeXe WHERE MaChuyenXe = @MaChuyenXe AND ThoiGianXuatPhat = @TGianXuatPhat AND SoGhe = @SoGhe";

                using (SqlCommand cmdSelect = new SqlCommand(query, con))
                {
                    cmdSelect.Parameters.AddWithValue("@MaChuyenXe", MaChuyen);
                    cmdSelect.Parameters.AddWithValue("@TGianXuatPhat", Departure);
                    cmdSelect.Parameters.AddWithValue("@SoGhe", sott);

                    object result = cmdSelect.ExecuteScalar();

                    if (result != null)
                    {
                        trangThai = Convert.ToInt32(result);
                    }
                }
            }

            if (trangThai == 1)
            {
                lstxedaban.Add(true);
            }
            else if (trangThai == 0)
            {
                lstxedaban.Add(false);
            }
        }
        public void GetSLGhe()
        {
            using (SqlConnection con = new SqlConnection(SQLConnect.Conn))
            {
                con.Open();

                string query = @$"SELECT SoLuongGhe FROM ChuyenXe WHERE MaChuyenXe = '{MaChuyen}' AND ThoiGianXuatPhat = '{Departure}'";

                using (SqlCommand cmdSelect = new SqlCommand(query, con))
                {

                    object result = cmdSelect.ExecuteScalar();

                    if (result != null)
                    {
                        SLGhe = Convert.ToInt32(result);
                    }
                }
            }
        }
        public void GetmaTuyen()
        {
            using (SqlConnection con = new SqlConnection(SQLConnect.Conn))
            {
                con.Open();

                string query = $"SELECT MaTuyen FROM ChuyenXe WHERE MaChuyenXe = '{MaChuyen}' AND ThoiGianXuatPhat = '{Departure}'";

                using (SqlCommand cmdSelect = new SqlCommand(query, con))
                {

                    object result = cmdSelect.ExecuteScalar();

                    if (result != null)
                    {
                        maTuyen = Convert.ToString(result);
                    }
                }
            }
        }
        public void GetGiaVe(DateTime NgayMua)
        {
            using (SqlConnection con = new SqlConnection(SQLConnect.Conn))
            {
                con.Open();
                string query = "SELECT TOP 1 GiaVe FROM QLGiaVe WHERE CONVERT(DATE, NgayApDung, 103) <= @NgayMua AND MaTuyen = @MaTuyen ORDER BY NgayApDung DESC";
                using (SqlCommand cmdSelect = new SqlCommand(query, con))
                {
                    cmdSelect.Parameters.AddWithValue("@NgayMua", Departure);
                    cmdSelect.Parameters.AddWithValue("@MaTuyen", maTuyen);

                    object result = cmdSelect.ExecuteScalar();
                    Console.WriteLine(query);
                    if (result != null)
                    {
                        GiaVe = Convert.ToInt32(result);
                    }
                }
            }
        }
        public void GetMaVeXe(int SoGhe)
        {
            using (SqlConnection con = new SqlConnection(SQLConnect.Conn))
            {
                con.Open();
                string query = "SELECT MaVeXe FROM VeXe WHERE SoGhe = @SoGhe AND MaChuyenXe = @MaChuyenXe AND ThoiGianXuatPhat = @TGXP";
                using (SqlCommand cmdSelect = new SqlCommand(query, con))
                {
                    cmdSelect.Parameters.AddWithValue("@SoGhe", SoGhe);
                    cmdSelect.Parameters.AddWithValue("@MaChuyenXe", MaChuyen);
                    cmdSelect.Parameters.AddWithValue("@TGXP", Departure);

                    object result = cmdSelect.ExecuteScalar();
                    if (result != null)
                    {
                        maVeXe = Convert.ToString(result);
                    }
                }
            }
        }
    }
}
