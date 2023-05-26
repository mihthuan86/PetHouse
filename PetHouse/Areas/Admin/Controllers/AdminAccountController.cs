using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetHouse.Areas.Admin.ViewModel;
using PetHouse.Data;
using PetHouse.Data.Setting;
using PetHouse.Data.Static;
using PetHouse.Models;
using PetHouse.ViewModel;
using System.Data;
using System.Drawing.Printing;

namespace PetHouse.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "admin,staff")]
	public class AdminAccountController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly PetHouseDbContext _context;
		public INotyfService _notyfService { get; }

		public AdminAccountController(UserManager<User> userManager, SignInManager<User> signInManager, PetHouseDbContext context, INotyfService notyfService)
		{
			_context = context;
			_userManager = userManager;
			_signInManager = signInManager;
			_notyfService = notyfService;

		}
		[Authorize(Roles = "admin")]
		[Route("Admin/TaotaikhoanNV.cshtml", Name = "TaoTKNV")]
		public IActionResult CreateNV()
		{
			return View(new CreateStaffVM());
		}
		[HttpPost]
		[Route("Admin/TaotaikhoanNV.cshtml", Name = "TaoTKNV")]
		public async Task<IActionResult> Register(CreateStaffVM taikhoan)
		{
			try
			{

				if (ModelState.IsValid)
				{
					var usere = await _userManager.FindByEmailAsync(taikhoan.Email);
					if (usere != null)
					{
						_notyfService.Error("Email này đã được sử dụng");
						return RedirectToAction("CreateNV");
					}
					var usern = await _userManager.FindByNameAsync(taikhoan.UserName);
					if (usern != null)
					{
						_notyfService.Error("User Name này đã được sử dụng");
						return RedirectToAction("CreateNV");

					}
					var users = await _userManager.FindByNameAsync(taikhoan.Phone);
					if (users != null)
					{
						_notyfService.Error("Số điện thoại này đã được sử dụng");
						return RedirectToAction("CreateNV");
						
					}
					var newUser = new User()
					{
						FullName = taikhoan.FullName,
						Email = taikhoan.Email,
						UserName = taikhoan.UserName,
						PhoneNumber = taikhoan.Phone,
						CreateDate = DateTime.Now,
						Status = 1,//active
						Address = taikhoan.Address
					};
					var newUserResponse = await _userManager.CreateAsync(newUser, taikhoan.Password);

					if (newUserResponse.Succeeded)
					{
						await _userManager.AddToRoleAsync(newUser, UserRoles.Staff);
						_notyfService.Success("Đăng ký thành công");
						return RedirectToAction("IndexNV", "AdminAccount");
					}
					else
					{
						_notyfService.Error("Sai định dạng mật khẩu");
						return RedirectToAction("CreateNV");
					}
				}
				else
				{
					_notyfService.Error("Đã có lỗi xảy ra");
					return RedirectToAction("CreateNV");
				}
			}
			catch
			{
				return View(taikhoan);
			}
		}
		[Authorize(Roles = "admin")]
		[Route("taikhoanNV.cshtml", Name = "QLTKNV")]
		public async Task<IActionResult> IndexNV(int? pageNumber)
		{
			int pageSize = 10;
			var staff = await _userManager.GetUsersInRoleAsync("staff");
			var sta = staff.AsQueryable();
			var paginatedStaff = PaginatedList<User>.Create(sta, pageNumber ?? 1, pageSize);
			return View(paginatedStaff);
		}
		[Route("taikhoanKH.cshtml", Name = "QLTKKH")]
		public async Task<IActionResult> IndexKH(int? pageNumber)
		{
			int pageSize = 10;
			var customer = await _userManager.GetUsersInRoleAsync("customer");
			var cus = customer.AsQueryable();
			var paginatedCustomer = PaginatedList<User>.Create(cus, pageNumber ?? 1, pageSize);
			return View(paginatedCustomer);
		}
		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Login", "Account", new { area = "" });
		}
		[HttpPost]
		[Route("/api/user/block")]
		public IActionResult Block(string id)
		{
			if (id == null)
			{
				return Json(new { success = false });
			}
			var user = _context.Users.FirstOrDefault(x => x.Id == id);
			if (user == null)
			{
				return Json(new { success = false });
			}
			user.Status = 0;
			user.UpdateDate = DateTime.Now;
			_context.Update(user);
			_context.SaveChanges();
			return Json(new { success = true });
		}
		[HttpPost]
		[Route("/api/user/active")]
		public IActionResult Active(string id)
		{
			if (id == null)
			{
				return Json(new { success = false });
			}
			var user = _context.Users.FirstOrDefault(x => x.Id == id);
			if (user == null)
			{
				return Json(new { success = false });
			}
			user.UpdateDate = DateTime.Now;
			user.Status = 1;
			_context.Update(user);
			_context.SaveChanges();
			return Json(new { success = true });
		}
		[Authorize(Roles = "admin")]
		public async Task<IActionResult> Edit(string id)
		{
			var user = _context.Users.FirstOrDefault(x => x.Id == id);
			var userVM = new UpdateUserVM()
			{
				Id = id,
				FullName = user.FullName,
				UserName = user.UserName,
				Address = user.Address,
				Phone = user.PhoneNumber,
				Email = user.Email
			};
			if (user == null)
			{
				return NotFound();
			}
			return View(userVM);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, [Bind("Id,FullName,UserName,Email,Phone,Address")] UpdateUserVM staff)
		{
			if (id != staff.Id) return NotFound();
			if (ModelState.IsValid)
			{
				var userEdit = _context.Users.FirstOrDefault(x => x.Id == id);
				if (userEdit == null) { return NotFound(); }
				userEdit.FullName = staff.FullName;
				userEdit.UserName = staff.UserName;
				userEdit.Address = staff.Address;
				userEdit.PhoneNumber = staff.Phone;
				userEdit.UpdateDate = staff.UpdateDate;
				_context.Users.Update(userEdit);
				await _context.SaveChangesAsync();
				_notyfService.Success("Chỉnh sửa thành công");

				return RedirectToAction(nameof(IndexNV));
			}
			return View(staff);
		}

		public IActionResult Details(string id,int? pageNumber)
		{
			var user = _context.Users.FirstOrDefault(x => x.Id == id);
			var userOrder = _context.Orders.Where(x => x.CustomerId == id).AsQueryable();
			ViewBag.Order = PaginatedList<Order>.Create(userOrder, pageNumber ?? 1, 10);
			return View(user);
		}
	}
}
