using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetHouse.Data;
using PetHouse.Models;
using PetHouse.ViewModel;
using System.Data;

namespace PetHouse.Controllers
{
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
        public IActionResult Login() => View(new LoginVM());
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail)
                ?? await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        var role = await _userManager.GetRolesAsync(user);
                        if (role.Contains("customer"))
                        {
                            return RedirectToAction("Index", "Home",new { area = ""});
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home", new { area = "Admin" });
                        }
                    }
                }
                TempData["Error"] = "Bạn đã nhập sai mật khẩu, vui lòng thử lại!!!";
                return View(loginVM);
            }

            TempData["Error"] = "Không có email hoặc username này trong hệ thống!!!";
            return View(loginVM);
        }
		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home", new { area = "" });
		}
	}
}
