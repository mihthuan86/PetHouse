using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetHouse.Data;
using PetHouse.Data.Setting;
using PetHouse.Models;
using System.Data;
using System.Drawing.Printing;

namespace PetHouse.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "admin,staff")]
	public class AdminOrderController : Controller
	{

		private readonly PetHouseDbContext _context;
		public INotyfService _notyfService { get; }

		public AdminOrderController(PetHouseDbContext context, INotyfService notyfService)
		{
			_context = context;
			_notyfService = notyfService;
		}
		[Route("Admin/DanhSachDonHang.cshtml")]
		public IActionResult Index(int? pageNumber, int? stt)
		{
			if (stt == null) { stt = 10; }
			IQueryable<Order> Orders;
			if (stt == 10)
			{
				Orders = _context.Orders.Include(o => o.User)
			.AsNoTracking()
				.OrderByDescending(x => x.OrderDate).AsQueryable();
				
			}
			else
			{
				Orders = _context.Orders.Include(o => o.User).Where(x=>x.OrderStatus==stt)
				.AsNoTracking()
					.OrderByDescending(x => x.OrderDate).AsQueryable();
			}
			ViewData["stt"] = stt;
			var listOrder = PaginatedList<Order>.Create(Orders, pageNumber ?? 1, 16);
			return View(listOrder);
		}
		[Route("Admin/ChiTietDonHang.cshtml")]
		public async Task<IActionResult> Details(int? id,string returnUrl)
		{
			if (id == null)
			{
				return NotFound();
			}

			var order = await _context.Orders
				.Include(o => o.User)				
				.FirstOrDefaultAsync(m => m.Id == id);
			if (order == null)
			{
				return NotFound();
			}

			var Chitietdonhang = _context.OrderDetails
				.Include(x => x.Product)
				.AsNoTracking()
				.Where(x => x.OrderId == order.Id)
				.OrderBy(x => x.ProductId)
				.ToList();
			ViewBag.ChiTiet = Chitietdonhang;
			ViewBag.ReturnUrl = returnUrl;
			return View(order);
		}		
		public async Task<IActionResult> ChangeStt(int OrderId,int stt)
		{
			var donhang = await _context.Orders.
				AsNoTracking().Include(x => x.User).
				FirstOrDefaultAsync(x => x.Id == OrderId);
			if (donhang == null) { return NotFound(); }
			var oldstt = donhang.OrderStatus;
			donhang.OrderStatus = stt;
			_context.Orders.Update(donhang);
			if (stt == 1)
			{
				var orderItems = _context.OrderDetails.Where(x=>x.OrderId == donhang.Id).ToList();

				
				foreach (var item in orderItems)
				{
					// Lấy thông tin sản phẩm từ cơ sở dữ liệu
					var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
					if (product.Quantity < item.Quantity)
					{
						_notyfService.Warning("Số lượng sản phẩm không đủ để thực hiện đơn hàng này.");
						return RedirectToAction("Details", new { id = OrderId });
					}
					// Trừ số lượng sản phẩm từ cơ sở dữ liệu
					product.Quantity -= item.Quantity;
					_context.Products.Update(product);

				}

				// Lưu các thay đổi vào cơ sở dữ liệu
				
			}
			if (stt == -1 && oldstt == 1)
			{
				var orderItems = _context.OrderDetails.Where(x => x.OrderId == donhang.Id).ToList();


				foreach (var item in orderItems)
				{
					// Lấy thông tin sản phẩm từ cơ sở dữ liệu
					var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
					
					// Trừ số lượng sản phẩm từ cơ sở dữ liệu
					product.Quantity += item.Quantity;
					_context.Products.Update(product);
				}

				// Lưu các thay đổi vào cơ sở dữ liệu
				
			}
			_context.SaveChanges();
			_notyfService.Success("Cập nhật trạng thái đơn hàng thành công");
			return RedirectToAction("Details", new { id = OrderId });
		}
	}
}
