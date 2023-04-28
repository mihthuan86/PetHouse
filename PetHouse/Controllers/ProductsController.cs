using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PetHouse.Data;
using PetHouse.Data.Setting;
using PetHouse.Models;
using PetHouse.ViewModel;
using System.Drawing.Printing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PetHouse.Controllers
{
	public class ProductsController : Controller
	{
		private readonly PetHouseDbContext _context;

		public ProductsController(PetHouseDbContext context)
		{
			_context = context;
		}
		public IActionResult Index(int? pageNumber)
		{
			ViewBag.Brand = _context.Brands.ToList();
			ViewBag.PCategory = _context.Categories.Where(x => x.isParent).ToList();
			ViewBag.Categories = _context.Categories.ToList();
			ViewBag.CateName = "Tất cả sản phẩm";
			int pageSize = 9;
			var products = _context.Products.Include(p => p.Brand).Include(p => p.Category).OrderByDescending(x => x.CreateDate).AsQueryable();
			var paginatedProducts = PaginatedList<Product>.Create(products, pageNumber ?? 1, pageSize);
			return View(paginatedProducts);
		}
		public async Task<IActionResult> Details(int Id)
		{
			ViewBag.Categories = _context.Categories.ToList();
			var model = await _context.Products.FirstOrDefaultAsync(x => x.Id == Id);
			if (model == null)
			{
				return RedirectToAction("Index");
			}

			ViewBag.SameProduct = _context.Products.Where(x => x.CategoryId == model.CategoryId && x.Id != Id).Take(4);
			return View(model);

		}
		public IActionResult List(int cateId)
		{
			ViewBag.Brand = _context.Brands.ToList();
			ViewBag.PCategory = _context.Categories.Where(x => x.isParent).ToList();
			ViewBag.Categories = _context.Categories.ToList();
			var cate = _context.Categories.FirstOrDefault(x => x.Id == cateId);
			if (cate == null) return NotFound();
			IQueryable<Product> products;
			ViewBag.CateName = cate.Name;
			if (cate.isParent)
			{
				products = _context.Products.
					Include(p => p.Brand).
					Include(p => p.Category).
					Where(p => p.Category.ParentId == cateId).
					AsQueryable();
			}
			else
			{
				products = _context.Products.
					Include(p => p.Brand).
					Include(p => p.Category).
					Where(p => p.Category.Id == cateId).
					AsQueryable();
			}
			products = products.OrderByDescending(x => x.CreateDate);
			int pageSize = 12;
			var paginatedProducts = PaginatedList<Product>.Create(products,1, pageSize);
			return View("Index",paginatedProducts);
		}
		public IActionResult Find(string keyword)
		{
            ViewBag.PCategory = _context.Categories.Where(x => x.isParent).ToList();
            ViewBag.Brand = _context.Brands.ToList();
            ViewBag.Categories = _context.Categories.ToList();
			IQueryable<Product> ls;
            if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
            {
                return RedirectToAction("Index");
            }
			ls = _context.Products.AsNoTracking()
								  .Include(a => a.Category)
								  .Where(x => x.Name.Contains(keyword))
								  .OrderByDescending(x => x.Name)
								  .AsQueryable();
            ViewBag.CateName = "Tìm kiếm cho ' " + keyword + " '";
            if (ls.Count()>0)
            {
                
                var paginatedProducts = PaginatedList<Product>.Create(ls, 1, 12);
                return View("Index", paginatedProducts);
            }
            else
            {
                return View("NotFind");
            }
        }
	}
}
