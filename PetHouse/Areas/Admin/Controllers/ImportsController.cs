using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetHouse.Data;
using PetHouse.Models;

namespace PetHouse.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ImportsController : Controller
    {
        private readonly PetHouseDbContext _context;

        public ImportsController(PetHouseDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Imports
        public async Task<IActionResult> Index()
        {
            var petHouseDbContext = _context.Imports.Include(i => i.Supplier).Include(i => i.User);
            return View(await petHouseDbContext.ToListAsync());
        }

        // GET: Admin/Imports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Imports == null)
            {
                return NotFound();
            }

            var import = await _context.Imports
                .Include(i => i.Supplier)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (import == null)
            {
                return NotFound();
            }

            return View(import);
        }

        // GET: Admin/Imports/Create
        public IActionResult Create()
        {
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Address");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Admin/Imports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SupplierId,UserId,ImportDate,PaymentStatus,Status")] Import import)
        {
            if (ModelState.IsValid)
            {
                _context.Add(import);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Address", import.SupplierId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", import.UserId);
            return View(import);
        }

        // GET: Admin/Imports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Imports == null)
            {
                return NotFound();
            }

            var import = await _context.Imports.FindAsync(id);
            if (import == null)
            {
                return NotFound();
            }
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Address", import.SupplierId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", import.UserId);
            return View(import);
        }

        // POST: Admin/Imports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SupplierId,UserId,ImportDate,PaymentStatus,Status")] Import import)
        {
            if (id != import.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(import);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImportExists(import.Id))
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
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Address", import.SupplierId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", import.UserId);
            return View(import);
        }

        // GET: Admin/Imports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Imports == null)
            {
                return NotFound();
            }

            var import = await _context.Imports
                .Include(i => i.Supplier)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (import == null)
            {
                return NotFound();
            }

            return View(import);
        }

        // POST: Admin/Imports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Imports == null)
            {
                return Problem("Entity set 'PetHouseDbContext.Imports'  is null.");
            }
            var import = await _context.Imports.FindAsync(id);
            if (import != null)
            {
                _context.Imports.Remove(import);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImportExists(int id)
        {
          return (_context.Imports?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
