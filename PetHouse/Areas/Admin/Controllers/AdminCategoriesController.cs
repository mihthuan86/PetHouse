using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetHouse.Data;
using PetHouse.Data.Setting;
using PetHouse.Models;
using PetHouse.ViewModel;

namespace PetHouse.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AdminCategoriesController : Controller
	{
		private readonly PetHouseDbContext _context;


		public AdminCategoriesController(PetHouseDbContext context)
		{
			_context = context;
		}

		[Route("Admin/danhmuc.cshtml")]
		// GET: Admin/Categories	
		public IActionResult Index(int? pageNumber)
		{
			ViewBag.PCate = _context.Categories.Where(x => x.isParent).ToList();
			int pageSize = 10;
			var products = _context.Categories.OrderByDescending(x => x.CreateDate).AsQueryable();
			var paginatedProducts = PaginatedList<Category>.Create(products, pageNumber ?? 1, pageSize);
			return View(paginatedProducts);
		}

		// GET: Admin/Categories/Details/5
		public async Task<IActionResult> Details(int? id, int? pageNumber)
		{
			if (id == null || _context.Categories == null)
			{
				return NotFound();
			}
			var category = await _context.Categories
				.FirstOrDefaultAsync(m => m.Id == id);
			if (category == null)
			{
				return NotFound();
			}
			if (!category.isParent)
			{
				var CateProduct = _context.Products.Where(x => x.CategoryId == id).AsQueryable();
				ViewBag.Product = PaginatedList<Product>.Create(CateProduct, pageNumber ?? 1, 10);
				ViewBag.Count = _context.Products.Where(x => x.CategoryId == id).Count();
				ViewBag.PCategory = new SelectList(_context.Categories.Where(n => n.isParent == true).OrderBy(n => n.Name).ToList(), "Id", "Name");
					
			}
			else
			{
				var CateProduct = _context.Products.Where(x => x.Category.ParentId == id).AsQueryable();
				ViewBag.Product = PaginatedList<Product>.Create(CateProduct, pageNumber ?? 1, 10);
				ViewBag.Count = _context.Products.Where(x => x.Category.ParentId == id).Count();
				
			}
			return View(category);
		}

		// GET: Admin/Categories/Create
		public IActionResult Create()
		{
			ViewBag.PCategory = new SelectList(_context.Categories.Where(n => n.isParent == true).OrderBy(n => n.Name).ToList(), "Id", "Name");
			return View();
		}

		// POST: Admin/Categories/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name,Description,ParentId,isParent")] Category category)
		{
			if (!ModelState.IsValid)
			{
				if (category.isParent)
				{
					category.ParentId = null;
				}
				category.UpdateDate = null;
				category.CreateDate = DateTime.Now;
				category.Status = 1;
				_context.Add(category);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(category);
		}

		// GET: Admin/Categories/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Categories == null)
			{
				return NotFound();
			}
			ViewBag.PCategory = new SelectList(_context.Categories.Where(n => n.isParent == true).OrderBy(n => n.Name).ToList(), "Id", "Name");
			var category = await _context.Categories.FindAsync(id);
			if (category == null)
			{
				return NotFound();
			}
			return View(category);
		}

		// POST: Admin/Categories/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ParentId")] Category category)
		{
			if (id != category.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					var CateEdit = _context.Categories.FirstOrDefault(x => x.Id == id);
					if (CateEdit == null) { return NotFound(); }
					CateEdit.Name = category.Name;
					CateEdit.Description = category.Description;
					CateEdit.ParentId = category.ParentId;
					CateEdit.UpdateDate = DateTime.Now;
					_context.Update(CateEdit);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!CategoryExists(category.Id))
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
			return View(category);
		}

		// GET: Admin/Categories/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Categories == null)
			{
				return NotFound();
			}

			var category = await _context.Categories
				.FirstOrDefaultAsync(m => m.Id == id);
			if (category == null)
			{
				return NotFound();
			}

			return View(category);
		}

		//// POST: Admin/Categories/Delete/5
		//[HttpPost, ActionName("Delete")]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Categories == null)
			{
				return Problem("Entity set 'PetHouseDbContext.Categories'  is null.");
			}
			var category = await _context.Categories.FindAsync(id);
			if (category != null)
			{
				category.Status = -1;
				if (category.isParent)
				{
					var cateChildren = await _context.Categories.Where(cate => cate.ParentId == id).ToListAsync();
					foreach (var cate in cateChildren)
					{
						cate.Status = -1;
					}
					_context.UpdateRange(cateChildren);
				}
				_context.Categories.Update(category);
			}
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> ActiveConfirmed(int id)
		{
			if (_context.Categories == null)
			{
				return Problem("Entity set 'PetHouseDbContext.Categories'  is null.");
			}
			var category = await _context.Categories.FindAsync(id);
			if (category != null)
			{
				category.Status = 1;
				if (!category.isParent)
				{
					var cateParent = await _context.Categories.FirstOrDefaultAsync(cate => cate.Id == category.ParentId);
					if (cateParent != null)
					{
						cateParent.Status = 1;
						_context.Update(cateParent);
					}
				}
				_context.Categories.Update(category);
			}
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		private bool CategoryExists(int id)
		{
			return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
