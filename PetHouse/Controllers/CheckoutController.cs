using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PetHouse.Data;
using PetHouse.Extention;
using PetHouse.Helpper;
using PetHouse.Models;
using PetHouse.ViewModel;

namespace PetHouse.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly PetHouseDbContext _context;
		private readonly UserManager<User> _userManager;
		public INotyfService _notyfService { get; }
		public CheckoutController(PetHouseDbContext context, INotyfService notyfService, UserManager<User> userManager)
		{
			_context = context;
			_notyfService = notyfService;
			_userManager = userManager;
		}
		public List<CartItem> GioHang
		{
			get
			{
				var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
				if (gh == default(List<CartItem>))
				{
					gh = new List<CartItem>();
				}
				return gh;
			}
		}
		[Route("checkout.html", Name = "Checkout")]
		public async Task<IActionResult> Index()
		{
			ViewBag.Categories = _context.Categories.ToList();

			var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
			User user;
			if (HttpContext.User.Identity.IsAuthenticated)
			{
				string username = HttpContext.User.Identity.Name;
				user = await _userManager.FindByNameAsync(username);
			}
			else
			{
				user = await _userManager.FindByNameAsync("customer");
			}
			MuaHangVM model = new MuaHangVM();
			model.CustomerId = user.Id;
			if (user != null && user.UserName != "customer")
			{
				model.FullName = user.FullName;
				model.Email = user.Email;
				model.Phone = user.PhoneNumber;				
				model.Address = user.Address;
			}
			ViewBag.GioHang = cart;
			return View(model);
		}
		[HttpPost]
		[Route("checkout.html", Name = "Checkout")]
		public async Task<IActionResult> Index(MuaHangVM model)
		{
			ViewBag.Categories = _context.Categories.ToList();
			//Lay ra gio hang de xu ly
			var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var customer = await _userManager.FindByIdAsync(model.CustomerId);
			if(customer != null && customer.UserName!="customer")
			{
				if(customer.Address == "")
				{
					customer.Address = model.Address;
					_context.Update(customer);
					_context.SaveChanges();
				}
			}
            ViewBag.GioHang = cart;			
			try
			{
				if (ModelState.IsValid)
				{
					//Khoi tao don hang
					Order donhang = new Order();
					donhang.CustomerId = model.CustomerId;
					donhang.Receiver_Address = model.Address;
					donhang.Receiver_PhoneNumber = model.Phone;
					donhang.Receiver_FullName = model.FullName;					
					donhang.Receiver_Email = model.Email;					
					donhang.Note = "Hello";
					donhang.OrderDate = DateTime.Now;
					donhang.OrderStatus = 0;//đơn hàng mới
					donhang.ShipDate = DateTime.Now.AddDays(7);
					donhang.PaymentId = model.PaymentID;
					donhang.TotalPrice = total(model.PaymentID, cart.Sum(x => x.TotalMonay));
					HttpContext.Session.Set("DonHang", donhang);
					ViewBag.DonHang = donhang;
					return View("ThanhToan");

				}
				else
				{
					return View(model);
				}
			}
				catch
			{
				return View(model);
			}
		}		
		public IActionResult ThanhToan()
		{
			ViewBag.Categories = _context.Categories.ToList();
			var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
			var donhang = HttpContext.Session.Get<Order>("DonHang");
			if (donhang != null)
			{
				
				_context.Add(donhang);
				_context.SaveChanges();

				//tao danh sach don hang

				foreach (var item in cart)
				{
					OrderDetail orderDetail = new OrderDetail();
					orderDetail.OrderId = donhang.Id;
					orderDetail.ProductId = item.Product.Id;
					orderDetail.Quantity = item.amount;
					orderDetail.Price = item.Product.OrderPrice;
					_context.Add(orderDetail);
				}
				_context.SaveChanges();
				//clear gio hang
				HttpContext.Session.Remove("GioHang");
				HttpContext.Session.Remove("DonHang");
				//Xuat thong bao
				_notyfService.Success("Đơn hàng đặt thành công");
				//cap nhat thong tin khach hang
				return View("OrderCompleted");
			}
			return View();
		}
		public double total(int id, double stotal)
		{
			double s_total= stotal;
			var COD = _context.Payments.FirstOrDefault(p => p.Id == 1);
			var Paypal = _context.Payments.FirstOrDefault(p => p.Id == 2);
			if (id == 1 )
			{
				s_total += COD.Surcharge;
			}
			if(id == 2)
			{

				s_total = s_total + s_total * Paypal.Surcharge + COD.Surcharge;
			}
			return s_total;
		}
	}
}
