using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_DienTu_LTDN.Data;
using Web_DienTu_LTDN.ViewModels;

namespace Web_DienTu_LTDN.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly NamShop2025Context db;
        public HangHoaController(NamShop2025Context context)
        {
            db = context;

        }
        public IActionResult Index(int? loai)
        {
            var hangHoas = db.HangHoas.AsQueryable();
            if (loai.HasValue)
            {
                hangHoas = hangHoas.Where(p => p.MaLoai == loai.Value);
            }
            var result = hangHoas.Select(p => new HangHoaViewModel
            {
                maHangHoa = p.MaHh,
                tenHangHoa = p.TenHh,
                donGia = p.DonGia ?? 0,
                hinhAnh = p.Hinh ?? "",
                moTaNgan = p.MoTaDonVi ?? "",
                tenLoai = p.MaLoaiNavigation.TenLoai

            });

            return View(result);
        }
        public IActionResult Search(string query)
        {
            var hangHoas = db.HangHoas.AsQueryable();
            if (query != null)
            {
                hangHoas = hangHoas.Where(p => p.TenHh.Contains(query));
            }
            var result = hangHoas.Select(p => new HangHoaViewModel
            {
                maHangHoa = p.MaHh,
                tenHangHoa = p.TenHh,
                donGia = p.DonGia ?? 0,
                hinhAnh = p.Hinh ?? "",
                moTaNgan = p.MoTaDonVi ?? "",
                tenLoai = p.MaLoaiNavigation.TenLoai

            });

            return View(result);

        }
        public IActionResult Detail(int id)
        {
            var data = db.HangHoas.Include(p => p.MaLoaiNavigation).SingleOrDefault(p => p.MaHh == id);
            if(data == null)
            {
                TempData["Message"] = $"Không thấy sản phẩm có mã {id}";
                return Redirect("/404");
            }
            var result = new ChiTietHangHoaViewModel
            {
                maHangHoa = data.MaHh,
                tenHangHoa = data.TenHh,
                donGia = data.DonGia ?? 0,
                chiTiet = data.MoTa ?? string.Empty,
                hinhAnh = data.Hinh ?? string.Empty,
                moTaNgan = data.MoTaDonVi ?? string.Empty,
                tenLoai = data.MaLoaiNavigation.TenLoai,
                soLuongTon = 10,//tính sau
                diemDanhGia = 5,//check sau
            };
            return View(result);

        }
    }
}
