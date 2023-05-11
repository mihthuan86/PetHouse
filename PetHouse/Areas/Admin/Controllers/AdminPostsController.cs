using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetHouse.Data;
using PetHouse.Data.Setting;
using PetHouse.Extension;
using PetHouse.Models;
using System.Security.Claims;

namespace PetHouse.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "admin,staff")]
	public class AdminPostsController : Controller
	{
		private readonly PetHouseDbContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public INotyfService _notyfService { get; }
		public AdminPostsController(PetHouseDbContext context, INotyfService notyfService, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_notyfService = notyfService;
			_webHostEnvironment = webHostEnvironment;
		}
		public IActionResult Index(int? pageNumber)
		{
			var collection = _context.Posts.ToList();
			foreach (var item in collection)
			{
				if (item.CreateDate == null)
				{
					item.CreateDate = DateTime.Now;
					_context.Update(item);
					_context.SaveChanges();
				}
			}
			var pageSize = 10;
			var lsTinDangs = _context.Posts
				.Include(x => x.User)
				.Include(x=>x.Menu)
				.OrderBy(x => x.Id)
				.AsQueryable();
			var paginatedPosts = PaginatedList<Post>.Create(lsTinDangs, pageNumber ?? 1, pageSize);
			ViewBag.CurrentPage = pageNumber;
			return View(paginatedPosts);
		}
		[HttpPost]
		public IActionResult Delete(int Id)
		{
			var post = _context.Posts.FirstOrDefault(x=> x.Id == Id);
			if(post == null)
			{
				return Json(new { success = false });
			}
			_context.Posts.Remove(post);
			_context.SaveChanges();
			return Json(new { success = true });
		}
		[HttpPost]
		public IActionResult ChangeStt(int Id,int stt)
		{
			var post = _context.Posts.FirstOrDefault(x => x.Id == Id);
			if (post == null)
			{
				return Json(new { success = false });
			}
			post.Status = stt;
			_context.Posts.Update(post);
			_context.SaveChanges();
			return Json(new { success = true });
		}
		public IActionResult Create()
		{
            ViewData["MenuId"] = new SelectList(_context.Menus, "Id", "Name");
            return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name,ImgFile,Description,Content,MenuId")] Post post)
		{
			if (!ModelState.IsValid)
			{
				string wwwRootPath = _webHostEnvironment.WebRootPath;
				string fileName = Path.GetFileNameWithoutExtension(post.ImgFile.FileName);
				string extension = Path.GetExtension(post.ImgFile.FileName);
				fileName += DateTime.Now.ToString("ddMMyyyy") + extension;
				post.ImgUrl = fileName;
				string path = Path.Combine(wwwRootPath + "/Admin/Img/Post", fileName);
				using (var fileStream = new FileStream(path, FileMode.Create))
				{
					await post.ImgFile.CopyToAsync(fileStream);
				}
				var user = HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
				post.AuthorId = user;
				post.CreateDate = DateTime.Now;
				post.Status = 1;
				_context.Add(post);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}			
			return View(post);
		}
	}
}
