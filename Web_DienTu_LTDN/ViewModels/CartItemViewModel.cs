namespace Web_DienTu_LTDN.ViewModels
{
    public class CartItemViewModel
    {
        public int maHangHoa { get; set; }
        public string tenHangHoa { get; set; }
        public string hinhAnh { get; set; }
        public double donGia { get; set; }
        
        public int soLuong {  get; set; }
        public double thanhTien => soLuong * donGia;


    }
}
