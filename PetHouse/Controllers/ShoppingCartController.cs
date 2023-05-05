using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using PetHouse.Data;
using PetHouse.Models;
using PetHouse.ViewModel;
using PetHouse.Extention;
using System;
using Microsoft.CodeAnalysis;

namespace PetHouse.Controllers
{
	public class ShoppingCartController : Controller
	{
		private readonly PetHouseDbContext _context;
		public INotyfService _notyfService { get; }

		public ShoppingCartController(PetHouseDbContext context, INotyfService notyfService)
		{
			_context = context;
			_notyfService = notyfService;
		}

		public HttpContext GetHttpContext()
		{
			return HttpContext;
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
		[HttpPost]
		[Route("api/cart/add")]
		public IActionResult AddToCart(int productID, int? amount)
		{
			List<CartItem> cart = GioHang;

			try
			{
				//Them san pham vao gio hang
				CartItem item = cart.SingleOrDefault(p => p.Product.Id == productID);
				if (item != null) // da co => cap nhat so luong
				{
					if (amount == null)
					{
						item.amount++;
					}
					else
					{
						item.amount = item.amount + amount.Value;
					}
					//luu lai session
					//HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
				}
				else
				{
					Product hh = _context.Products.SingleOrDefault(p => p.Id == productID);
					item = new CartItem
					{
						amount = amount.HasValue ? amount.Value : 1,
						Product = hh
					};
					cart.Add(item);//Them vao gio
				}

				//Luu lai Session
				HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
				return Json(new { success = true });
			}
			catch
			{
				return Json(new { success = false });
			}
		}

		[HttpPost]
		[Route("api/cart/remove")]
		public ActionResult Remove(int productID)
		{

			try
			{
				List<CartItem> gioHang = GioHang;
				CartItem item = gioHang.SingleOrDefault(p => p.Product.Id == productID);
				if (item != null)
				{
					gioHang.Remove(item);
				}
				//luu lai session
				HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);
				return Json(new { success = true });
			}
			catch
			{
				return Json(new { success = false });
			}
		}
		[HttpPost]
		[Route("api/cart/plus")]
		public ActionResult Plus(int productID)
		{

			try
			{
				List<CartItem> gioHang = GioHang;
				CartItem item = gioHang.SingleOrDefault(p => p.Product.Id == productID);
				if (item != null)
				{
					item.amount++;
				}
				//luu lai session
				HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);
				return Json(new { success = true });
			}
			catch
			{
				return Json(new { success = false });
			}
		}
		[HttpPost]
		[Route("api/cart/minus")]
		public ActionResult minus(int productID)
		{

			try
			{
				List<CartItem> gioHang = GioHang;
				CartItem item = gioHang.SingleOrDefault(p => p.Product.Id == productID);
				if (item != null)
				{
					item.amount--;
					if (item.amount < 1)
					{
						gioHang.Remove(item);
					}
				}
				//luu lai session
				HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);
				return Json(new { success = true });
			}
			catch
			{
				return Json(new { success = false });
			}
		}
		[HttpPost]
		[Route("api/cart/update")]
		public IActionResult UpdateCart(int productID, int? amount)
		{
			//Lay gio hang ra de xu ly
			var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
			try
			{
				if (cart != null)
				{
					CartItem item = cart.SingleOrDefault(p => p.Product.Id == productID);
					if (item != null && amount.HasValue) // da co -> cap nhat so luong
					{
						item.amount = amount.Value;
						//if (item.amount < 1)
						//{
						//	cart.Remove(item);
						//}
					}
					//Luu lai session
					HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
				}
				return Json(new { success = true });
			}
			catch
			{
				return Json(new { success = false });
			}
		}
		[Route("cart.html", Name = "Cart")]
		public IActionResult Index()
		{
			ViewBag.Categories = _context.Categories.ToList();
			return View(GioHang);
		}

		public IActionResult OrderNow(int productID)
		{
			List<CartItem> cart = GioHang;

			try
			{
				//Them san pham vao gio hang
				CartItem item = cart.SingleOrDefault(p => p.Product.Id == productID);
				if (item != null) // da co => cap nhat so luong
				{

					item.amount++;

				}
				else
				{
					Product hh = _context.Products.SingleOrDefault(p => p.Id == productID);
					item = new CartItem
					{
						amount = 1,
						Product = hh
					};
					cart.Add(item);//Them vao gio
				}

				//Luu lai Session
				HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
				return RedirectToAction("Index", "Checkout");
			}
			catch
			{
				return Json(new { success = false });
			}
		}
	}
}
