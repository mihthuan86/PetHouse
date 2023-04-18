using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetHouse.Data;
using PetHouse.Data.Static;
using PetHouse.Helpper;
using PetHouse.Models;
using PetHouse.ViewModel;
using System.Data;
using System.Security.Claims;

namespace PetHouse.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly PetHouseDbContext _context;
        public INotyfService _notyfService { get; }

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, PetHouseDbContext context, INotyfService notyfService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _notyfService = notyfService;
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
                if (user.Status == 0)
                {
                    _notyfService.Error("Bạn đã bị chặn khỏi hệ thống");
                    return View(loginVM);
                }
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        var role = await _userManager.GetRolesAsync(user);
                        if (role.Contains("customer"))
                        {
                            _notyfService.Success("Chào mừng bạn trở lại");
                            return RedirectToAction("Index", "Home", new { area = "" });
                        }
                        else
                        {
                            _notyfService.Success("Chào mừng bạn tới trang quản trị");
                            return RedirectToAction("Index", "Home", new { area = "Admin" });
                        }
                    }
                }
                _notyfService.Error("Bạn đã nhập sai mật khẩu, vui lòng thử lại!!!");
                return View(loginVM);
            }

            _notyfService.Error("Không có email hoặc username này trong hệ thống!!!");
            return View(loginVM);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        [Route("dang-ky.html", Name = "DangKy")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "DangKy")]
        public async Task<IActionResult> Register(RegisterVM taikhoan)
        {
            try
            {
               
                if (ModelState.IsValid)
                {
                    var usere = await _userManager.FindByEmailAsync(taikhoan.Email);
                    if (usere != null)
                    {
                        _notyfService.Error("Email này đã được sử dụng");
                        return View(taikhoan);
                    }
                    var usern = await _userManager.FindByNameAsync(taikhoan.UserName);
                    if (usern != null)
                    {
                        _notyfService.Error("User Name này đã được sử dụng");
                        return View(taikhoan);
                    }
                    var users = await _userManager.FindByNameAsync(taikhoan.Phone);
                    if (users != null)
                    {
                        _notyfService.Error("Số điện thoại này đã được sử dụng");
                        return View(taikhoan);
                    }
                    var newUser = new User()
                    {
                        FullName = taikhoan.FullName,
                        Email = taikhoan.Email,
                        UserName = taikhoan.UserName,
                        PhoneNumber = taikhoan.Phone,
                        CreateDate = DateTime.Now,
                        Status = 1,//active
                        Address = ""
                    };
                    var newUserResponse = await _userManager.CreateAsync(newUser, taikhoan.Password);

                    if (newUserResponse.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(newUser, UserRoles.Customer);
                        _notyfService.Success("Đăng ký thành công");
                        return RedirectToAction("Login","Account");
                    }
                    else
                    {
                        _notyfService.Error(newUserResponse.Errors.ToString());
                        return View(taikhoan);
                    }
                }
                else
                {
                    return View(taikhoan);
                }
            }
            catch
            {
                return View(taikhoan);
            }
        }
    }
}
