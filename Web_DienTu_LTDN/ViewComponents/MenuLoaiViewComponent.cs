using Microsoft.AspNetCore.Mvc;
using Web_DienTu_LTDN.Data;
using Web_DienTu_LTDN.ViewModels;

namespace Web_DienTu_LTDN.ViewComponents
{
    public class MenuLoaiViewComponent : ViewComponent
    {
        private NamShop2025Context db;

        public MenuLoaiViewComponent(NamShop2025Context context) => db = context;
        
        public IViewComponentResult Invoke()
        {
            var data = db.Loais.Select(loai => new MenuLoaiViewModel
            {
                MaLoai = loai.MaLoai,
                TenLoai = loai.TenLoai,
                SoLuong = loai.HangHoas.Count
            }).OrderBy(p => p.TenLoai);
            return View(data);
        }
    }
}
