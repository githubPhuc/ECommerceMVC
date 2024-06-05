using AutoMapper;
using ECommerceMVC.Data;
using ECommerceMVC.Helpers;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    
    public class AdminController : Controller
    {

        private readonly Hshop2023Context db;
        private readonly IMapper _mapper;

        public AdminController(Hshop2023Context context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }
        public async Task<IActionResult> DangNhap(LoginVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == model.UserName);
                if (khachHang == null)
                {
                    ModelState.AddModelError("loi", "Không có khách hàng này");
                }
                else
                {
                    if (!khachHang.HieuLuc)
                    {
                        ModelState.AddModelError("loi", "Tài khoản đã bị khóa. Vui lòng liên hệ Admin.");
                    }
                    else
                    {
                        if (khachHang.MatKhau != model.Password.ToMd5Hash(khachHang.RandomKey))
                        {
                            ModelState.AddModelError("loi", "Sai thông tin đăng nhập");
                        }
                        else
                        {
                            var claims = new List<Claim> {
                        new Claim(ClaimTypes.Email, khachHang.Email),
                        new Claim(ClaimTypes.Name, khachHang.HoTen),
                        new Claim(MySetting.CLAIM_CUSTOMERID, khachHang.MaKh),

                        //claim - role động
                        new Claim(ClaimTypes.Role, "Customer")
                    };

                            if (khachHang.VaiTro == 1) // Nếu khách hàng có vai trò là admin
                            {
                                claims.RemoveAll(c => c.Type == ClaimTypes.Role); // Xóa claim về vai trò khách hàng
                                claims.Add(new Claim(ClaimTypes.Role, "Admin")); // Thêm claim về vai trò admin
                            }

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                            await HttpContext.SignInAsync(claimsPrincipal);

                            if (Url.IsLocalUrl(ReturnUrl))
                            {
                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                if (khachHang.VaiTro == 1) // Nếu khách hàng có vai trò là admin
                                {
                                    return RedirectToAction("Index","Admin"); // Chuyển hướng đến trang chủ admin
                                }
                                else
                                {
                                    return Redirect("/");
                                }
                            }
                        }
                    }
                }
            }
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
		[Authorize]
		public async Task<IActionResult> DangXuat()
		{
			await HttpContext.SignOutAsync();
			return Redirect("/");
		}
	}
}
