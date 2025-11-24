using Microsoft.AspNetCore.Mvc;
using Web_DienTu_LTDN.Data;
using Web_DienTu_LTDN.Helpers;
using Web_DienTu_LTDN.ViewModels;

namespace Web_DienTu_LTDN.ViewComponents
{
    public class CartViewComponent:ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get < List <CartItemViewModel >> (MySetting.Cart_Key) ?? new List<CartItemViewModel>();
            return View("CartPanel", new CartModel
            {
                Quantity = cart.Sum(p => p.soLuong),
                Total = cart.Sum (p => p.thanhTien)
            });
        }
    }
}
