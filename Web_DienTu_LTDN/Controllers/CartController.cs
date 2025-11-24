using Microsoft.AspNetCore.Mvc;
using Web_DienTu_LTDN.Data;
using Web_DienTu_LTDN.ViewModels;
using Web_DienTu_LTDN.Helpers;

namespace Web_DienTu_LTDN.Controllers
{
    public class CartController : Controller
    {
        private NamShop2025Context db;

        public CartController( NamShop2025Context context) {
            db = context;
        } 
        public List<CartItemViewModel> Cart => HttpContext.Session.Get < List < CartItemViewModel >> (MySetting.Cart_Key) ?? new List<CartItemViewModel>();
        public IActionResult Index()
        {
            return View(Cart);
        }
        public IActionResult AddToCart(int id , int quantity = 1)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p=> p.maHangHoa == id);
            if (item == null)
            {
                var hangHoa = db.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if(hangHoa == null)
                {
                    TempData["Message"] = $"Khong tim thay hang hoa co ma {id}";
                    return Redirect("/404");
                }
                item = new CartItemViewModel
                {
                    maHangHoa = hangHoa.MaHh,
                    tenHangHoa = hangHoa.TenHh,
                    donGia = hangHoa.DonGia ?? 0,
                    hinhAnh = hangHoa.Hinh ?? String.Empty,
                    soLuong = quantity
                };
                gioHang.Add(item);
            }
            else
            {
                item.soLuong += quantity;
            }
            HttpContext.Session.Set(MySetting.Cart_Key, gioHang);
            return RedirectToAction("Index");
        }
        public IActionResult RemoveCart(int id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.maHangHoa == id);
            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(MySetting.Cart_Key, gioHang);             
            }
            return RedirectToAction("Index");
        }
    }
}
