using Microsoft.AspNetCore.Mvc;
using PetHouse.Data;
using PetHouse.Models;
using System.Diagnostics;

namespace PetHouse.Controllers
{	
	public class HomeController : Controller
	{
		private readonly PetHouseDbContext _context;

		public HomeController(PetHouseDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{

			var cho = _context.Products.Where(x => x.Category.ParentId == 16).Take(6).ToList();
			var meo = _context.Products.Where(x => x.Category.ParentId == 17).Take(6).ToList();
			var model = new Tuple<List<Product>, List<Product>>(cho, meo);
			ViewBag.Categories = _context.Categories.ToList();
			return View(model);
		}

		public IActionResult Privacy()
		{
			return View();
		}
		public IActionResult Service()
		{
			ViewBag.Categories = _context.Categories.ToList();
			return View();
		}
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}