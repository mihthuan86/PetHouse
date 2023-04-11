using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetHouse.Data;
using PetHouse.Data.Setting;
using PetHouse.Models;

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
			ViewBag.PCategory = _context.Categories.Where(x=>x.isParent).ToList();
			ViewBag.Categories = _context.Categories.ToList();
            int pageSize = 9;
            var products = _context.Products.Include(p => p.Brand).Include(p => p.Category).OrderByDescending(x => x.CreateDate).AsQueryable();
            var paginatedProducts = PaginatedList<Product>.Create(products, pageNumber ?? 1, pageSize);
            return View(paginatedProducts);			
		}
		public async Task<IActionResult> Details(int Id)
		{
			ViewBag.Categories = _context.Categories.ToList();		
			var model = await _context.Products.FirstOrDefaultAsync(x=> x.Id == Id);
			ViewBag.SameProduct = _context.Products.Where(x => x.CategoryId == model.CategoryId && x.Id!=Id).Take(5);
			return View(model);
		}
	}
}
