using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetHouse.Data;
using PetHouse.Data.Setting;
using PetHouse.Models;
using System.Data;

namespace PetHouse.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "admin,staff")]

	public class AdminMenusController : Controller
	{
		
		private readonly PetHouseDbContext _context;
		public INotyfService _notyfService { get; }
        public AdminMenusController( PetHouseDbContext context,INotyfService notyfService)
        {
			_context = context;
			_notyfService = notyfService;
        }
        public async Task<IActionResult> Index()
		{
			return _context.Menus != null ?
						  View(await _context.Menus.ToListAsync()) :
						  Problem("Entity set 'PetHouseDbContext.Brands'  is null.");
		}
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Menus == null)
			{
				return NotFound();
			}

			var memu  = await _context.Menus
				.FirstOrDefaultAsync(m => m.Id == id);
			if (memu == null)
			{
				return NotFound();
			}
			return View(memu);
		}

		// GET: Admin/Brands/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Admin/Brands/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name")] Menu menu)
		{
			if (ModelState.IsValid)
			{
				menu.CreateDate = DateTime.Now;
				menu.UpdateDate = null;
				menu.Status = 1;
				_context.Add(menu);
				await _context.SaveChangesAsync();
				_notyfService.Success("Thêm danh mục bài viết thành công");
				return RedirectToAction(nameof(Index));
			}
			return View(menu);
		}

		// GET: Admin/Brands/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Menus == null)
			{
				return NotFound();
			}

			var brand = await _context.Menus.FindAsync(id);
			if (brand == null)
			{
				return NotFound();
			}
			return View(brand);
		}

		// POST: Admin/Brands/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Menu menu)
		{
			if (id != menu.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{

					menu.Status = 1;
					menu.UpdateDate = DateTime.Now;
					_context.Update(menu);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!BrandExists(menu.Id))
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
			return View(menu);
		}

		// GET: Admin/Brands/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Brands == null)
			{
				return NotFound();
			}

			var brand = await _context.Brands
				.FirstOrDefaultAsync(m => m.Id == id);
			if (brand == null)
			{
				return NotFound();
			}

			return View(brand);
		}

		//// POST: Admin/Brands/Delete/5
		//[HttpPost, ActionName("Delete")]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Menus == null)
			{
				return Problem("Entity set 'PetHouseDbContext.Brands'  is null.");
			}
			var brand = await _context.Menus.FindAsync(id);
			if (brand != null)
			{
				brand.Status = -1;
				_context.Menus.Update(brand);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> ActiveConfirmed(int id)
		{
			if (_context.Menus == null)
			{
				return Problem("Entity set 'PetHouseDbContext.Brands'  is null.");
			}
			var brand = await _context.Menus.FindAsync(id);
			if (brand != null)
			{
				brand.Status = 1;
				_context.Menus.Update(brand);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		private bool BrandExists(int id)
		{
			return (_context.Brands?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
