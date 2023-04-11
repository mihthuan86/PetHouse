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
			ViewBag.Categories = _context.Categories.ToList();
			return View();
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