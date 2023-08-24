using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FixedAssetsStockTaking.Data;
using FixedAssetsStockTaking.Models;
using static FixedAssetsStockTaking.ViewModels.UserVM;

namespace FixedAssetsStockTaking.Controllers
{
    public class ExtraLinesController : Controller
    {
        private readonly FixedAssetsStockTakingContext _context;

        public ExtraLinesController(FixedAssetsStockTakingContext context)
        {
            _context = context;
        }

        // GET: ExtraLines
        public async Task<IActionResult> Index()
        {
            var fixedAssetsStockTakingContext = _context.ExtraLines.Include(e => e.Inventar);
            return View(await fixedAssetsStockTakingContext.ToListAsync());
        }

        // GET: ExtraLines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ExtraLines == null)
            {
                return NotFound();
            }

            var extraLine = await _context.ExtraLines
                .Include(e => e.Inventar)
                .FirstOrDefaultAsync(m => m.InventarID == id);
            if (extraLine == null)
            {
                return NotFound();
            }

            return View(extraLine);
        }

        // GET: ExtraLines/Create
        public IActionResult Create(int? id)
        {
            if (id == null || _context.ExtraLines == null)
            {
                return NotFound();
            }
            ViewData["InventarID"] = new SelectList(_context.Inventar, "ID", "ID");
            var inventars = _context.Inventar.Where(i => i.ID == id).ToList();
            ViewData["Inventar"] = inventars;
            return View();
        }

        // POST: ExtraLines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InventarID,MFLine")] ExtraLine extraLine)
        {
            if (extraLine.MFLine == null)
            {
                return RedirectToAction("RevenueCenters", "Inventars", new { id = extraLine.InventarID });
            }

            var existingMij = await _context.MijlocFix
                                         .Where(m => m.Line == extraLine.MFLine && m.InventarID == extraLine.InventarID)
                                         .AnyAsync();

            var existingExtra = await _context.ExtraLines
                                         .Where(m => m.MFLine == extraLine.MFLine && m.InventarID == extraLine.InventarID)
                                         .AnyAsync();

            if (existingMij || existingExtra)
            {
                return RedirectToAction("RevenueCenters", "Inventars", new { id = extraLine.InventarID });
            }

            if (ModelState.IsValid)
            {
                _context.Add(extraLine);
                await _context.SaveChangesAsync();
                return RedirectToAction("RevenueCenters", "Inventars", new { id = extraLine.InventarID });
            }

            ViewData["InventarID"] = new SelectList(_context.Inventar, "ID", "ID", extraLine.InventarID);
            return View(extraLine);
        }

        // GET: ExtraLines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ExtraLines == null)
            {
                return NotFound();
            }

            var extraLine = await _context.ExtraLines.FindAsync(id);

            if (extraLine == null)
            {
                return NotFound();
            }

            ViewData["InventarID"] = new SelectList(_context.Inventar, "ID", "ID", extraLine.InventarID);
            return View(extraLine);
        }

        // POST: ExtraLines/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InventarID,MFLine")] ExtraLine extraLine)
        {
            if (id != extraLine.InventarID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(extraLine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExtraLineExists(extraLine.InventarID))
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
            ViewData["InventarID"] = new SelectList(_context.Inventar, "ID", "ID", extraLine.InventarID);
            return View(extraLine);
        }

        // GET: ExtraLines/Delete/5
        public async Task<IActionResult> Delete(string line, int? id)
        {
            if (id == null || line == null)
            {
                return NotFound();
            }

            var extraLine = await _context.ExtraLines
                .Include(e => e.Inventar)
                .FirstOrDefaultAsync(m => m.InventarID == id && m.MFLine == line) ;
            if (extraLine == null)
            {
                return NotFound();
            }

            return View(extraLine);
        }

        // POST: ExtraLines/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed([Bind("MFLine,InventarID")] ExtraLine extra)
        {
            var extraLine = await _context.ExtraLines.FindAsync(extra.InventarID, extra.MFLine);

            if (extraLine == null)
            {
                return NotFound();
            }

            _context.ExtraLines.Remove(extraLine);
            await _context.SaveChangesAsync();
            return RedirectToAction("RevenueCenters", "Inventars", new { id = extra.InventarID });
        }

        private bool ExtraLineExists(int id)
        {
          return (_context.ExtraLines?.Any(e => e.InventarID == id)).GetValueOrDefault();
        }
    }
}
