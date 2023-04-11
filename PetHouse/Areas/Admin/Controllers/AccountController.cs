using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetHouse.Data;
using PetHouse.Models;
using System.Data;

namespace PetHouse.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles ="admin,staff")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly PetHouseDbContext _context;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, PetHouseDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> AccountNV()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("staff");
            return View(usersInRole); 
        }
        public IActionResult CreateNV()
        {
            return View();
        }
        public async Task<IActionResult> AccountKH()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("customer");
            return View(usersInRole);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account",new { area = "" });
        }

    }
}
