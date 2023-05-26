using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetHouse.Data;
using PetHouse.Models;

namespace PetHouse.Controllers
{
	public class SearchController : Controller
	{
		private readonly PetHouseDbContext _context;
		public SearchController(PetHouseDbContext context)
        {
			_context = context;
        }
		[HttpPost]
		public IActionResult FindProduct(string keyword)
		{
			List<Product> ls = new List<Product>();
			if (string.IsNullOrEmpty(keyword) || keyword.Length < 1)
			{
				return PartialView("ListProductsSearchPartial", null);
			}
			ls = _context.Products.AsNoTracking()
								  .Include(a => a.Category)
								  .Where(x => x.Name.Contains(keyword)&&x.Quantity>0&&x.Status!=-1)
								  .OrderByDescending(x => x.Name)
								  .Take(10)
								  .ToList();
			if (ls == null)
			{
				return PartialView("ListProductsSearchPartial", null);
			}
			else
			{
				return PartialView("ListProductsSearchPartial", ls);
			}
		}
	}
}
