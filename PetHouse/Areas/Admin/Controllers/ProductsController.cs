using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetHouse.Data;
using PetHouse.Data.Setting;
using PetHouse.ViewModel;
using PetHouse.Models;

namespace PetHouse.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly PetHouseDbContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ProductsController(PetHouseDbContext context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}

		// GET: Admin/Products
		public IActionResult Index(int? pageNumber)
        {
			ViewBag.CategoryParent = _context.Categories.Where(x=>x.isParent).ToList();
			int pageSize = 5;
			var products = _context.Products.Include(p => p.Brand).Include(p => p.Category).OrderByDescending(x => x.CreateDate).AsQueryable();
			var paginatedProducts = PaginatedList<Product>.Create(products, pageNumber ?? 1, pageSize);           
            return View(paginatedProducts);
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
		public IActionResult GetChildCategories(int parentId)
		{
			var childCategories = _context.Categories.Where(c => c.ParentId == parentId).ToList();
			return Json(childCategories);
		}
		// GET: Admin/Products/Create
		public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(c=>c.isParent==true), "Id", "Name");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ImgFile,Description,CategoryId,BrandId,OrderPrice,Quantity")] Product product)
        {
			if (!ModelState.IsValid)
			{
				string wwwRootPath = _webHostEnvironment.WebRootPath;
				string fileName = Path.GetFileNameWithoutExtension(product.ImgFile.FileName);
				string extension = Path.GetExtension(product.ImgFile.FileName);
				fileName += DateTime.Now.ToString("ddMMyyyy") + extension;
				product.ProductImgAvt = fileName;
				string path = Path.Combine(wwwRootPath + "/Admin/Img/ProductAvt", fileName);
				using (var fileStream = new FileStream(path, FileMode.Create))
				{
					await product.ImgFile.CopyToAsync(fileStream);
				}
                product.ImgDetails = "12345";
                product.CreateDate = DateTime.Now;
                product.Status = 1;
				_context.Add(product);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
			ViewData["CategoryId"] = new SelectList(_context.Categories.Where(c => c.isParent == true), "Id", "Name");
			return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Description", product.CategoryId);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CategoryId,BrandId,OrderPrice,ImnportPrice,Quantity,Status,CreateDate,UpdateDate")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Description", product.CategoryId);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

		// POST: Admin/Products/Delete/5
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Products == null)
			{
				return Problem("Entity set 'PetHouseDbContext.Brands'  is null.");
			}
			var product = await _context.Products.FindAsync(id);
			if (product != null)
			{
				product.Status = -1;
				_context.Products.Update(product);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> ActiveConfirmed(int id)
		{
			if (_context.Products == null)
			{
				return Problem("Entity set 'PetHouseDbContext.Brands'  is null.");
			}
			var product = await _context.Products.FindAsync(id);
			if (product != null)
			{
				product.Status = 1;
				_context.Products.Update(product);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
