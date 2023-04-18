﻿using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetHouse.Data;
using PetHouse.Models;
using PetHouse.ViewModel;

namespace PetHouse.Controllers
{
    public class OrderController : Controller
    {       
        private readonly PetHouseDbContext _context;
        private readonly UserManager<User> _userManager;
        public INotyfService _notyfService { get; }
        public OrderController(UserManager<User> userManager,PetHouseDbContext context,INotyfService notyfService)
        {

            _userManager = userManager;
            _context = context;
            _notyfService = notyfService;

        }       
        public async Task<IActionResult> Index(int? stt)
        {
            ViewBag.Categories = _context.Categories.ToList();
            User user;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string username = HttpContext.User.Identity.Name;
                user = await _userManager.FindByNameAsync(username);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            if(stt == null)
            {
				return NotFound();
			}
            List<Order> lsDonHang;
            string returnStt="";
            switch (stt)
            {
                case 10:
                    {
						lsDonHang = await _context.Orders
					   .AsNoTracking()
					   .Where(x => x.CustomerId == user.Id)
					   .OrderByDescending(x => x.OrderDate)
					   .ToListAsync();
						returnStt = "Tất cả đơn hàng";
                        break;
					}
				case 1:
					{
						lsDonHang = await _context.Orders
					   .AsNoTracking()
					   .Where(x => x.CustomerId == user.Id && x.OrderStatus == stt)
					   .OrderByDescending(x => x.OrderDate)
					   .ToListAsync();
						returnStt = "Đơn hàng đang giao";
						break;
					}
				case 2:
					{
						lsDonHang = await _context.Orders
					   .AsNoTracking()
					   .Where(x => x.CustomerId == user.Id && x.OrderStatus == stt)
					   .OrderByDescending(x => x.OrderDate)
					   .ToListAsync();
						returnStt = "Đơn hàng đã hoàn thành ";
						break;
					}
				case 0:
					{
						lsDonHang = await _context.Orders
					   .AsNoTracking()
					   .Where(x => x.CustomerId == user.Id && x.OrderStatus == stt)
					   .OrderByDescending(x => x.OrderDate)
					   .ToListAsync();
						returnStt = "Đơn hàng đã đặt";
						break;
					}
				case -1:
					{
						lsDonHang = await _context.Orders
					   .AsNoTracking()
					   .Where(x => x.CustomerId == user.Id && x.OrderStatus == stt)
					   .OrderByDescending(x => x.OrderDate)
					   .ToListAsync();
						returnStt = "Đơn hàng đã hủy";
						break;
					}
				default: return NotFound();
			}

            ViewBag.Stt = returnStt;
			return View(lsDonHang);
        }		
		[HttpPost]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {                            
                var donhang = await _context.Orders                   
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (donhang == null) return NotFound();

                var chitietdonhang = _context.OrderDetails
                    .Include(x => x.Product)
                    .AsNoTracking()
                    .Where(x => x.OrderId == id)                    
                    .ToList();
                XemDonHang donHang = new XemDonHang();
                donHang.DonHang = donhang;
                donHang.ChiTietDonHang = chitietdonhang;
                return PartialView("Details", donHang);

            }
            catch
            {
                return NotFound();
            }
        }
    }
}
