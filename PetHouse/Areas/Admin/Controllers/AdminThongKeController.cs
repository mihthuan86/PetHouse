using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetHouse.Areas.Admin.ViewModel;
using PetHouse.Data;
using PetHouse.Models;
using System.Linq;

namespace PetHouse.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "admin")]

	public class AdminThongKeController : Controller
	{
		private readonly PetHouseDbContext _context;
		public AdminThongKeController(PetHouseDbContext context)
		{
			_context = context;
		}
		[HttpGet]
		[Route("thongke/day")]
		public IActionResult Index(int? stt)
		{
			if (stt == null || stt == 0)
			{
				DateTime orderDay = DateTime.Today;
				var ordersToday = _context.Orders.Where(order => order.OrderDate.Date == orderDay).
					Where(order => order.PaymentId==2 || (order.PaymentId==1 && order.OrderStatus==2)).ToList();
				ViewBag.OrderCount = ordersToday.Count();
				ViewBag.COD = ordersToday.Where(x => x.PaymentId == 1).Count();
				ViewBag.Onl = ordersToday.Where(x => x.PaymentId == 2).Count();
				ViewBag.TotalMoney = ordersToday.Sum(x => x.TotalPrice);
				ViewBag.stt = 0;
				ViewBag.Top5 = ordersToday
					.Join(_context.OrderDetails,
					  order => order.Id,
					  orderDetail => orderDetail.OrderId,
					  (order, orderDetail) => new
					  {
						  orderDetail.ProductId,
						  orderDetail.Quantity
					  })
				.GroupBy(od => od.ProductId)
				.Select(g => new
				{
					ProductID = g.Key,
					TotalQuantity = g.Sum(od => od.Quantity)
				})
				.OrderByDescending(g => g.TotalQuantity)
				.Take(5)
				.Join(_context.Products,
					  product => product.ProductID,
					  p => p.Id,
					  (product, p) => new TKProductVM { Id = p.Id, Name = p.Name, Price = p.OrderPrice, Total = product.TotalQuantity })
				.ToList();

			}
			if (stt == 1)
			{
				DateTime orderDay = DateTime.Today.AddDays(-1);
				var ordersToday = _context.Orders.Where(order => order.OrderDate.Date == orderDay).
					Where(order => order.PaymentId == 2 || (order.PaymentId == 1 && order.OrderStatus == 2)).ToList();
				ViewBag.OrderCount = ordersToday.Count();
				ViewBag.COD = ordersToday.Where(x => x.PaymentId == 1).Count();
				ViewBag.Onl = ordersToday.Where(x => x.PaymentId == 2).Count();
				ViewBag.TotalMoney = ordersToday.Sum(x => x.TotalPrice);

				ViewBag.stt = 1;
				ViewBag.Top5 = ordersToday
					.Join(_context.OrderDetails,
					  order => order.Id,
					  orderDetail => orderDetail.OrderId,
					  (order, orderDetail) => new
					  {
						  orderDetail.ProductId,
						  orderDetail.Quantity
					  })
				.GroupBy(od => od.ProductId)
				.Select(g => new
				{
					ProductID = g.Key,
					TotalQuantity = g.Sum(od => od.Quantity)
				})
				.OrderByDescending(g => g.TotalQuantity)
				.Take(5)
				.Join(_context.Products,
					  product => product.ProductID,
					  p => p.Id,
					  (product, p) => new TKProductVM
					  {
						  Id = p.Id,
						  Name = p.Name,
						  Price = p.OrderPrice,
						  Total = product.TotalQuantity
					  })
				.ToList();
			}
			if (stt == 2)
			{
				DateTime startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1);
				DateTime endOfWeek = startOfWeek.AddDays(6);
				var ordersThisWeek = _context.Orders.Where(order => order.OrderDate.Date >= startOfWeek && order.OrderDate.Date <= endOfWeek).
					Where(order => order.PaymentId == 2 || (order.PaymentId == 1 && order.OrderStatus == 2)).ToList();
				ViewBag.OrderCount = ordersThisWeek.Count();
				ViewBag.COD = ordersThisWeek.Where(x => x.PaymentId == 1).Count();
				ViewBag.Onl = ordersThisWeek.Where(x => x.PaymentId == 2).Count();
				ViewBag.TotalMoney = ordersThisWeek.Sum(x => x.TotalPrice);
				ViewBag.stt = 2;
				ViewBag.Top5 = ordersThisWeek
					.Join(_context.OrderDetails,
					  order => order.Id,
					  orderDetail => orderDetail.OrderId,
					  (order, orderDetail) => new
					  {
						  orderDetail.ProductId,
						  orderDetail.Quantity
					  })
				.GroupBy(od => od.ProductId)
				.Select(g => new
				{
					ProductID = g.Key,
					TotalQuantity = g.Sum(od => od.Quantity)
				})
				.OrderByDescending(g => g.TotalQuantity)
				.Take(5)
				.Join(_context.Products,
					  product => product.ProductID,
					  p => p.Id,
					  (product, p) => new TKProductVM
					  {
						  Id = p.Id,
						  Name = p.Name,
						  Price = p.OrderPrice,
						  Total = product.TotalQuantity
					  })
				.ToList();
			}
			if (stt == 3)
			{
				DateTime startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
				DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
				var ordersThisMonth = _context.Orders.Where(order => order.OrderDate.Date >= startOfMonth && order.OrderDate.Date <= endOfMonth).
					Where(order => order.PaymentId == 2 || (order.PaymentId == 1 && order.OrderStatus == 2)).ToList()	;
				ViewBag.OrderCount = ordersThisMonth.Count();
				ViewBag.COD = ordersThisMonth.Where(x => x.PaymentId == 1).Count();
				ViewBag.Onl = ordersThisMonth.Where(x => x.PaymentId == 2).Count();
				ViewBag.TotalMoney = ordersThisMonth.Sum(x => x.TotalPrice);
				ViewBag.stt = 3;
				ViewBag.Top5 = ordersThisMonth
					.Join(_context.OrderDetails,
					  order => order.Id,
					  orderDetail => orderDetail.OrderId,
					  (order, orderDetail) => new
					  {
						  orderDetail.ProductId,
						  orderDetail.Quantity
					  })
				.GroupBy(od => od.ProductId)
				.Select(g => new
				{
					ProductID = g.Key,
					TotalQuantity = g.Sum(od => od.Quantity)
				})
				.OrderByDescending(g => g.TotalQuantity)
				.Take(5)
				.Join(_context.Products,
					  product => product.ProductID,
					  p => p.Id,
					  (product, p) => new TKProductVM
					  {
						  Id = p.Id,
						  Name = p.Name,
						  Price = p.OrderPrice,
						  Total = product.TotalQuantity
					  })
				.ToList();
			}
			return View();
		}
		[HttpPost]
		[Route("thongke/daytoday")]
		public ActionResult GetStatistical(string fromDate, string toDate)
		{
			DateTime startOfMonth = DateTime.Parse(fromDate);
			DateTime endOfMonth = DateTime.Parse(toDate);
			var ordersThisMonth = _context.Orders.Where(order => order.OrderDate.Date >= startOfMonth && order.OrderDate.Date <= endOfMonth);
			ViewBag.OrderCount = ordersThisMonth.Count();
			ViewBag.COD = ordersThisMonth.Where(x => x.PaymentId == 1).Count();
			ViewBag.Onl = ordersThisMonth.Where(x => x.PaymentId == 2).Count();
			ViewBag.TotalMoney = ordersThisMonth.Sum(x => x.TotalPrice);
			ViewBag.stt = -1;
			ViewBag.Top5 = ordersThisMonth
				.Join(_context.OrderDetails,
				  order => order.Id,
				  orderDetail => orderDetail.OrderId,
				  (order, orderDetail) => new
				  {
					  orderDetail.ProductId,
					  orderDetail.Quantity
				  })
			.GroupBy(od => od.ProductId)
			.Select(g => new
			{
				ProductID = g.Key,
				TotalQuantity = g.Sum(od => od.Quantity)
			})
			.OrderByDescending(g => g.TotalQuantity)
			.Take(5)
			.Join(_context.Products,
				  product => product.ProductID,
				  p => p.Id,
				  (product, p) => new TKProductVM
				  {
					  Id = p.Id,
					  Name = p.Name,
					  Price = p.OrderPrice,
					  Total = product.TotalQuantity
				  })
			.ToList();
			return View("Index");
		}
	}
}
