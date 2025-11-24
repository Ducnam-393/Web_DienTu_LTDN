namespace Web_DienTu_LTDN.ViewModels
{
    public class HangHoaViewModel
    {
        public int maHangHoa {  get; set; }
        public string tenHangHoa { get; set; }
        public string hinhAnh {  get; set; }
        public double donGia {  get; set; }
        public string moTaNgan {  get; set; }
        public string tenLoai {  get; set; }

    }
    public class ChiTietHangHoaViewModel
    {
        public int maHangHoa { get; set; }
        public string tenHangHoa { get; set; }
        public string hinhAnh { get; set; }
        public double donGia { get; set; }
        public string moTaNgan { get; set; }
        public string tenLoai { get; set; }
        public string chiTiet { get; set; }
        public int diemDanhGia { get; set; }
        public int soLuongTon { get; set; }

    }
}
