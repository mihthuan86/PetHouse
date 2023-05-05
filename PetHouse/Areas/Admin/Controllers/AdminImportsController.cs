using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetHouse.Data;
using PetHouse.Data.Setting;
using PetHouse.Models;

namespace PetHouse.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "admin,staff")]
	public class AdminImportsController : Controller
	{
		private readonly PetHouseDbContext _context;
		private readonly UserManager<User> _userManager;
		public AdminImportsController(PetHouseDbContext context, UserManager<User> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		// GET: Admin/Imports
		[Route("Admin/NhapHang.cshtml")]
		public IActionResult Index(int? pageNumber)
		{
			ViewBag.PCate = _context.Categories.Where(x => x.isParent).ToList();
			int pageSize = 10;
			var import = _context.Imports.OrderByDescending(x => x.ImportDate).
				Include(x => x.User).
				Include(x => x.Supplier).
				AsQueryable();
			var paginatedProducts = PaginatedList<Import>.Create(import, pageNumber ?? 1, pageSize);
			return View(paginatedProducts);
		}

		// GET: Admin/Imports/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Imports == null)
			{
				return NotFound();
			}

			var import = await _context.Imports
				.Include(i => i.Supplier)
				.Include(i => i.User)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (import == null)
			{
				return NotFound();
			}

			return View(import);
		}

		// GET: Admin/Imports/Create
		public IActionResult Create()
		{
			Import item = new Import();
			ImportDetail importDetail = new ImportDetail();
			importDetail.Id = 1;
			item.ImportDetails.Add(importDetail);
			ViewBag.Product = GetProducts();
			ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name");
			return View(item);
		}

		// POST: Admin/Imports/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Import import)
		{
			try
			{
				var user = await _userManager.FindByNameAsync("admin");
				Import item = new Import();
				item.SupplierId = import.SupplierId;		
				item.UserId = user.Id;
				item.ImportDate = null;
				item.Status = 0;
				item.TotalMoney = import.ImportDetails.Sum(x => x.Total);
				_context.Imports.Add(item);
				await _context.SaveChangesAsync();
				foreach (var details in import.ImportDetails)
				{
					ImportDetail importDetail = new ImportDetail();
					importDetail.Id = details.Id;
					importDetail.ImportId = item.Id;
					importDetail.ProductId = details.ProductId;
					importDetail.Price = details.Price;
					importDetail.Quantity = details.Quantity;
					_context.ImportDetails.Add(importDetail);
					await _context.SaveChangesAsync();
				}				
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				ViewBag.Product = GetProducts();
				ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name");
				return View(import);
			}
		}

		// GET: Admin/Imports/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Imports == null)
			{
				return NotFound();
			}

			var import = await _context.Imports.FindAsync(id);
			if (import == null)
			{
				return NotFound();
			}
			ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Address", import.SupplierId);
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", import.UserId);
			return View(import);
		}

		// POST: Admin/Imports/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,SupplierId,UserId,ImportDate,PaymentStatus,Status")] Import import)
		{
			if (id != import.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(import);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ImportExists(import.Id))
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
			ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Address", import.SupplierId);
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", import.UserId);
			return View(import);
		}

		// GET: Admin/Imports/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Imports == null)
			{
				return NotFound();
			}

			var import = await _context.Imports
				.Include(i => i.Supplier)
				.Include(i => i.User)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (import == null)
			{
				return NotFound();
			}

			return View(import);
		}

		// POST: Admin/Imports/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Imports == null)
			{
				return Problem("Entity set 'PetHouseDbContext.Imports'  is null.");
			}
			var import = await _context.Imports.FindAsync(id);
			if (import != null)
			{
				_context.Imports.Remove(import);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ImportExists(int id)
		{
			return (_context.Imports?.Any(e => e.Id == id)).GetValueOrDefault();
		}
		private List<SelectListItem> GetProducts()
		{
			var products = new List<SelectListItem>();
			products = _context.Products.Select(x => new SelectListItem()
			{
				Value = x.Id.ToString(),
				Text = x.Name
			}).ToList();
			var defItem = new SelectListItem()
			{
				Value = "",
				Text = "---Chọn sản phẩm----"
			};
			products.Insert(0, defItem);
			return products;
		}
	}
}
