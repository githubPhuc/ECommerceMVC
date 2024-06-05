using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECommerceMVC.Controllers
{
	public class HangHoaController : Controller
	{
		private readonly Hshop2023Context db;

		public HangHoaController(Hshop2023Context conetxt)
		{
			db = conetxt;
		}

		public IActionResult Index(int? loai, string sortOrder)
		{
			var hangHoas = db.HangHoas.AsQueryable();

			if (loai.HasValue)
			{
				hangHoas = hangHoas.Where(p => p.MaLoai == loai.Value);
			}
			switch (sortOrder)
			{
				case "name_asc":
					hangHoas = hangHoas.OrderBy(p => p.TenHh);
					break;
				case "name_desc":
					hangHoas = hangHoas.OrderByDescending(p => p.TenHh);
					break;
				case "price_asc":
					hangHoas = hangHoas.OrderBy(p => p.DonGia);
					break;
				case "price_desc":
					hangHoas = hangHoas.OrderByDescending(p => p.DonGia);
					break;
				default:
					break;
			}
			var result = hangHoas.Select(p => new HangHoaVM
			{
				MaHh = p.MaHh,
				TenHH = p.TenHh,
				DonGia = p.DonGia ?? 0,
				Hinh = p.Hinh ?? "",
				MoTaNgan = p.MoTaDonVi ?? "",
				TenLoai = p.MaLoaiNavigation.TenLoai
			});
			return View(result);
		}

		public IActionResult Search(string? query)
		{
			var hangHoas = db.HangHoas.AsQueryable();

			if (query != null)
			{
				hangHoas = hangHoas.Where(p => p.TenHh.Contains(query));
			}

			var result = hangHoas.Select(p => new HangHoaVM
			{
				MaHh = p.MaHh,
				TenHH = p.TenHh,
				DonGia = p.DonGia ?? 0,
				Hinh = p.Hinh ?? "",
				MoTaNgan = p.MoTaDonVi ?? "",
				TenLoai = p.MaLoaiNavigation.TenLoai
			});
			return View(result);
		}


		public IActionResult Detail(int id)
		{
			var data = db.HangHoas
				.Include(p => p.MaLoaiNavigation)
				.SingleOrDefault(p => p.MaHh == id);
			if (data == null)
			{
				TempData["Message"] = $"Không thấy sản phẩm có mã {id}";
				return Redirect("/404");
			}

			var result = new ChiTietHangHoaVM
			{
				MaHh = data.MaHh,
				TenHH = data.TenHh,
				DonGia = data.DonGia ?? 0,
				ChiTiet = data.MoTa ?? string.Empty,
				Hinh = data.Hinh ?? string.Empty,
				MoTaNgan = data.MoTaDonVi ?? string.Empty,
				TenLoai = data.MaLoaiNavigation.TenLoai,
				SoLuongTon = 10,//tính sau
				DiemDanhGia = 5,//check sau
                Reviews = data.Reviews.Select(r => new ReviewVM
                {
                    Id = r.Id,
                    Title = r.Title,
                    Content = r.Content,
                    Rating = r.Rating,
                    Date = r.Date
                }).ToList()
            };
			return View(result);
		}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(int hangHoaId, [Bind("Title,Content,Rating")] Review review)
        {
            if (ModelState.IsValid)
            {
                review.HangHoaId = hangHoaId;
                review.Date = DateTime.Now;
                db.Reviews.Add(review);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Detail), new { id = hangHoaId });
            }
            return View(review);
        }
		
	}
}
	

