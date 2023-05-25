using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetHouse.Areas.Admin.ViewModel;
using PetHouse.Data;
using PetHouse.Data.Setting;
using PetHouse.Extention;
using PetHouse.Models;
using PetHouse.ViewModel;

namespace PetHouse.Areas.Admin.Controllers
{
	[Area("Admin")]
	//[Authorize(Roles = "admin,staff")]
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
			var import = _context.Imports.OrderByDescending(x=>x.Id).
				Include(x => x.User).
				Include(x => x.Supplier).
				AsQueryable();
			var paginatedProducts = PaginatedList<Import>.Create(import, pageNumber ?? 1, pageSize);
			return View(paginatedProducts);
		}

		// GET: Admin/Imports/Details/5
		public IActionResult SelectProduct(int? pageNumber)
		{
			int pageSize = 5;
			var products = _context.Products.OrderBy(x => x.Quantity).Where(x => x.Status != -1).AsQueryable();
			var paginatedProducts = PaginatedList<Product>.Create(products, pageNumber ?? 1, pageSize);
			ViewBag.CurrentPage = pageNumber;
			HttpContext.Session.Remove("PhieuNhap");
			return View(paginatedProducts);
		}
		// GET: Admin/Imports/Create
		public IActionResult Create()
		{
			Import item = new Import();
			foreach (ImportItem importItem in PhieuNhap)
			{
				ImportDetail detail = new ImportDetail();
				detail.ProductId = importItem.Product.Id;
				detail.Product = importItem.Product;
				item.ImportDetails.Add(detail);
			}
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
				item.TotalMoney = 0;
				_context.Imports.Add(item);
				await _context.SaveChangesAsync();
				foreach (var details in import.ImportDetails)
				{
					ImportDetail importDetail = new ImportDetail();
					importDetail.ImportId = item.Id;
					importDetail.ProductId = details.ProductId;
					importDetail.Price = details.Price;
					importDetail.Quantity = details.Quantity;
					importDetail.Total = details.Price * details.Quantity;
					_context.ImportDetails.Add(importDetail);
					await _context.SaveChangesAsync();
				}
				item.TotalMoney = _context.ImportDetails.Where(x => x.ImportId == item.Id).Sum(x => x.Total);
				_context.Update(item);
				await _context.SaveChangesAsync();
				HttpContext.Session.Remove("PhieuNhap");
				var ip = _context.Imports.OrderByDescending(x => x.Id).First();
				return RedirectToAction("Details", new { Id = ip.Id });
			}
			catch
			{
				ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name");
				return View(import);
			}
		}
		public IActionResult Details(int Id)
		{
			var Import = _context.Imports.FirstOrDefault(x => x.Id == Id);
			ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name");
			ViewBag.Item = _context.ImportDetails.Where(x => x.ImportId == Id).Include(x => x.Product).ToList();
			return View(Import);
		}


		public List<ImportItem> PhieuNhap
		{
			get
			{
				var gh = HttpContext.Session.Get<List<ImportItem>>("PhieuNhap");
				if (gh == default(List<ImportItem>))
				{
					gh = new List<ImportItem>();
				}
				return gh;
			}
		}
		[HttpPost]
		[Route("api/import/add")]
		public IActionResult AddToCart(int productID)
		{
			List<ImportItem> cart = PhieuNhap;

			try
			{
				ImportItem it = cart.SingleOrDefault(p => p.Product.Id == productID);
				if (it == null)
				{
					Product hh = _context.Products.SingleOrDefault(p => p.Id == productID);
					ImportItem item = new ImportItem
					{
						Product = hh
					};
					cart.Add(item);
					HttpContext.Session.Set<List<ImportItem>>("PhieuNhap", cart);
				}
				return Json(new { success = true });
			}
			catch
			{
				return Json(new { success = false });
			}
		}

		[HttpPost]
		[Route("api/import/remove")]
		public ActionResult Remove(int productID)
		{

			try
			{
				List<ImportItem> cart = PhieuNhap;
				ImportItem item = PhieuNhap.SingleOrDefault(p => p.Product.Id == productID);
				if (item != null)
				{
					cart.Remove(item);
				}
				//luu lai session
				HttpContext.Session.Set<List<ImportItem>>("PhieuNhap", cart);
				return Json(new { success = true });
			}
			catch
			{
				return Json(new { success = false });
			}
		}
		public ActionResult Search(string? keyword)
		{
			if(keyword == null)
			{
				return RedirectToAction(nameof(Index));
			}
			int pageSize = 5;
			var products = _context.Products.OrderBy(x => x.Quantity).Where(x => x.Status != -1 && x.Name.Contains(keyword)).AsQueryable();
			var paginatedProducts = PaginatedList<Product>.Create(products,1, pageSize);
			ViewBag.keyword = keyword;
			return View("SelectProduct",paginatedProducts);
		}
		public IActionResult Confirm(int Id)
		{
			var import = _context.Imports.FirstOrDefault(p => p.Id == Id);
			var ipDetails = _context.ImportDetails.Where(x => x.ImportId == Id).ToList();
			if (import != null)
			{
				import.Status = 1;
				import.ImportDate = DateTime.Now;
				_context.Update(import);
				foreach (var item in ipDetails)
				{
					var product = _context.Products.FirstOrDefault(x => x.Id == item.ProductId);
					product.Quantity = product.Quantity + item.Quantity;
					_context.Update(product);
				}
				_context.SaveChanges();
			}
			return RedirectToAction(nameof(Index));
		}
	}
}
