using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceMVC.Data;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceMVC.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class KhachHangController : Controller
	{
		private readonly Hshop2023Context _context;

		public KhachHangController(Hshop2023Context context)
		{
			_context = context;
		}

		// GET: KhachHang
		public async Task<IActionResult> Index()
		{
			return View(await _context.KhachHangs.ToListAsync());
		}

		// GET: KhachHang/Details/5
		public async Task<IActionResult> Details(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var khachHang = await _context.KhachHangs
				.FirstOrDefaultAsync(m => m.MaKh == id);
			if (khachHang == null)
			{
				return NotFound();
			}

			return View(khachHang);
		}

		// GET: KhachHang/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: KhachHang/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("MaKh,MatKhau,HoTen,GioiTinh,NgaySinh,DiaChi,DienThoai,Email,Hinh,HieuLuc,VaiTro,RandomKey")] KhachHang khachHang)
		{
			if (ModelState.IsValid)
			{
				_context.Add(khachHang);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(khachHang);
		}

		// GET: KhachHang/Edit/5
		public async Task<IActionResult> Edit(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var khachHang = await _context.KhachHangs.FindAsync(id);
			if (khachHang == null)
			{
				return NotFound();
			}
			return View(khachHang);
		}

		// POST: KhachHang/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, [Bind("MaKh,MatKhau,HoTen,GioiTinh,NgaySinh,DiaChi,DienThoai,Email,Hinh,HieuLuc,VaiTro,RandomKey")] KhachHang khachHang)
		{
			if (id != khachHang.MaKh)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(khachHang);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!KhachHangExists(khachHang.MaKh))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(khachHang);
		}

		// GET: KhachHang/Delete/5
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var khachHang = await _context.KhachHangs
				.FirstOrDefaultAsync(m => m.MaKh == id);
			if (khachHang == null)
			{
				return NotFound();
			}

			return View(khachHang);
		}

		// POST: KhachHang/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			var khachHang = await _context.KhachHangs.FindAsync(id);
			_context.KhachHangs.Remove(khachHang);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool KhachHangExists(string id)
		{
			return _context.KhachHangs.Any(e => e.MaKh == id);
		}
	}
}
