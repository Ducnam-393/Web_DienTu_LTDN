using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Web_DienTu_LTDN.Data;
using Web_DienTu_LTDN.Helpers;
using Web_DienTu_LTDN.ViewModels;
using AutoMapper;

namespace Web_DienTu_LTDN.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly NamShop2025Context db;
        private readonly IMapper _mapper;

        public KhachHangController(NamShop2025Context context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        #region Đăng Ký (Code cũ của bạn giữ nguyên)
        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DangKy(RegisterVM model, IFormFile Hinh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var khachHang = _mapper.Map<KhachHang>(model);
                    khachHang.RandomKey = MyUtil.GenerateRamdomKey();
                    khachHang.MatKhau = model.MatKhau.ToMd5Hash(khachHang.RandomKey);
                    khachHang.HieuLuc = true;
                    khachHang.VaiTro = 0;

                    if (Hinh != null)
                    {
                        khachHang.Hinh = MyUtil.UploadHinh(Hinh, "KhachHang");
                    }

                    db.Add(khachHang);
                    db.SaveChanges();
                    return RedirectToAction("DangNhap", "KhachHang"); // Chuyển đến đăng nhập
                }
                catch (Exception ex)
                {
                    // Log lỗi
                }
            }
            return View(model);
        }
        #endregion

        #region Đăng Nhập
        [HttpGet]
        public IActionResult DangNhap(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DangNhap(LoginVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                // Tìm khách hàng theo MaKh (Tên đăng nhập)
                var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == model.UserName);

                if (khachHang == null)
                {
                    ModelState.AddModelError("Lỗi", "Không có khách hàng này");
                }
                else
                {
                    if (!khachHang.HieuLuc)
                    {
                        ModelState.AddModelError("Lỗi", "Tài khoản đã bị khóa. Vui lòng liên hệ Admin.");
                    }
                    else
                    {
                        // Kiểm tra mật khẩu (kết hợp mã hóa)
                        if (khachHang.MatKhau != model.Password.ToMd5Hash(khachHang.RandomKey))
                        {
                            ModelState.AddModelError("Lỗi", "Sai thông tin đăng nhập");
                        }
                        else
                        {
                            // Đăng nhập thành công -> Tạo Cookie
                            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, khachHang.Email),
                        new Claim(ClaimTypes.Name, khachHang.HoTen),
                        new Claim("CustomerId", khachHang.MaKh),
                        new Claim(ClaimTypes.Role, "Customer")
                    };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                            await HttpContext.SignInAsync(claimsPrincipal);

                            // --- XỬ LÝ CHUYỂN HƯỚNG ---
                            if (Url.IsLocalUrl(ReturnUrl))
                            {
                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                // Chuyển về trang chủ
                                return RedirectToAction("Index", "Home");
                            }
                        }
                    }
                }
            }
            return View(model); // Nếu chạy xuống đây nghĩa là đăng nhập thất bại
        }
        #endregion

        #region Đăng Xuất
        [Route("/DangXuat")]
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}