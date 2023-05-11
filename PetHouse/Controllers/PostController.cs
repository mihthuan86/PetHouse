using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetHouse.Data;
using PetHouse.Data.Setting;
using PetHouse.Models;

namespace PetHouse.Controllers
{
	public class PostController : Controller
	{
		private readonly PetHouseDbContext _context;
        public PostController(PetHouseDbContext context)
        {
			_context = context;
        }
        public IActionResult Index(int? pageNumber)
		{
			ViewBag.Categories = _context.Categories.ToList();
			ViewBag.Menu = _context.Menus.ToList();
            ViewBag.CateName = "Tất cả bài đăng";
            int pageSize = 9;
			var posts = _context.Posts.Include(p => p.User).Include(p => p.Menu).OrderByDescending(x => x.CreateDate).AsQueryable();
			var paginatedProducts = PaginatedList<Post>.Create(posts, pageNumber ?? 1, pageSize);
			return View(paginatedProducts);
		}
		public IActionResult Details(int? id)
		{
			if(id == null)
			{
				return NotFound();
			}
			ViewBag.Categories = _context.Categories.ToList();
			var post = _context.Posts.FirstOrDefault(x => x.Id == id);
			if(post == null) { return NotFound(); }
			return View(post);
		}
	}
}
