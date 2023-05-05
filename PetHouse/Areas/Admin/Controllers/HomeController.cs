using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetHouse.Data;
using PetHouse.Models;

namespace PetHouse.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "admin,staff")]
	public class HomeController : Controller
	{
		private readonly PetHouseDbContext _context;
		private readonly UserManager<User> _userManager;

		public HomeController( PetHouseDbContext context,UserManager<User> userManager)
        {
			_context = context;
			_userManager = userManager;
        }
        public async Task<IActionResult> Index()
		{
			var staffUsers = await _userManager.GetUsersInRoleAsync("customer");
			ViewBag.countAccount = staffUsers.Count();
			ViewBag.countProduct = _context.Products.Count();
            ViewBag.countOrder = _context.Orders.Count();
            ViewBag.countOrderCancel = _context.Orders.Where(x=>x.OrderStatus==-1).Count();
            ViewBag.ProductEmty = _context.Products.Where(x => x.Quantity < 5).ToList();
			ViewBag.TotalPrice = _context.Orders.Where(x => x.OrderStatus == 2).Sum(x => x.TotalPrice);
			ViewBag.newOrder = _context.Orders.Where(x => x.OrderStatus == 0).Include(x=>x.User).ToList();
            return View();
		}
	}
}
