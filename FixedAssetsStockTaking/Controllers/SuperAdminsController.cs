using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FixedAssetsStockTaking.Data;
using FixedAssetsStockTaking.Models;
using Microsoft.AspNetCore.Authorization;

namespace FixedAssetsStockTaking.Controllers
{

    public class SuperAdminsController : Controller
    {
        private readonly FixedAssetsStockTakingContext _context;

        public SuperAdminsController(FixedAssetsStockTakingContext context)
        {
            _context = context;
        }

        // GET: SuperAdmins
        public async Task<IActionResult> Index()
        {
              return _context.SuperAdmin != null ? 
                          View(await _context.SuperAdmin.ToListAsync()) :
                          Problem("Entity set 'FixedAssetsStockTakingContext.SuperAdmin'  is null.");
        }

        // GET: SuperAdmins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SuperAdmin == null)
            {
                return NotFound();
            }

            var superAdmin = await _context.SuperAdmin
                .FirstOrDefaultAsync(m => m.ID == id);
            if (superAdmin == null)
            {
                return NotFound();
            }

            return View(superAdmin);
        }

        // GET: SuperAdmins/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreateModal()
        {
            var superi = _context.SuperAdmin.ToList();
            ViewData["sups"] = superi;
            return View("CreateUser");
        }


        // POST: SuperAdmins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateModal([Bind("ID,Name")] SuperAdmin superAdmin)
        {
            //////////////////////////////////////////////////////////
            var User = Environment.UserName;
            var Superadmins = _context.SuperAdmin.ToList();
            bool isSuperAdmin = Superadmins.Any(admin => admin.Name == User);
            if (!isSuperAdmin)
            {
                return RedirectToAction("Restricted", "Inventars");
            }
            //////////////////////////////////////////////////////////
            if (ModelState.IsValid)
            {
                _context.Add(superAdmin);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Inventars");
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", "Inventars");
        }

        // GET: SuperAdmins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SuperAdmin == null)
            {
                return NotFound();
            }

            var superAdmin = await _context.SuperAdmin.FindAsync(id);
            if (superAdmin == null)
            {
                return NotFound();
            }
            return View(superAdmin);
        }

        // POST: SuperAdmins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] SuperAdmin superAdmin)
        {
            if (id != superAdmin.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(superAdmin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuperAdminExists(superAdmin.ID))
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
            return View(superAdmin);
        }

        // GET: SuperAdmins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SuperAdmin == null)
            {
                return NotFound();
            }

            var superAdmin = await _context.SuperAdmin
                .FirstOrDefaultAsync(m => m.ID == id);
            if (superAdmin == null)
            {
                return NotFound();
            }

            return View(superAdmin);
        }

        // POST: SuperAdmins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SuperAdmin == null)
            {
                return Problem("Entity set 'FixedAssetsStockTakingContext.SuperAdmin'  is null.");
            }
            var superAdmin = await _context.SuperAdmin.FindAsync(id);
            if (superAdmin != null)
            {
                _context.SuperAdmin.Remove(superAdmin);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuperAdminExists(int id)
        {
          return (_context.SuperAdmin?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
