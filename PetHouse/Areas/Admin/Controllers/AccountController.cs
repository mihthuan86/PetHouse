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
       
        public IActionResult CreateNV()
        {
            return View();
        }
        [Route("taikhoan.cshtml",Name ="QLTK")]
        public async Task<IActionResult> Index()
        {
            var staff = await _userManager.GetUsersInRoleAsync("staff");
            var customer = await _userManager.GetUsersInRoleAsync("staff");
            var model = new Tuple<IList<User>, IList<User>>(staff, customer);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account",new { area = "" });
        }

    }
}
