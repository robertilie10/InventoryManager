using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FixedAssetsStockTaking.Data;
using FixedAssetsStockTaking.Models;
using System.Security.Claims;

namespace FixedAssetsStockTaking.Controllers
{
    public class UsersController : Controller
    {
        private readonly FixedAssetsStockTakingContext _context;

        public UsersController(FixedAssetsStockTakingContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var fixedAssetsStockTakingContext = _context.Users.Include(u => u.Inventar).Include(u => u.Role);
            return View(await fixedAssetsStockTakingContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string username, int inventarId)
        {
            if (username == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Inventar)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Username == username && m.InventarID == inventarId);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create(int id)
        {       
            ViewData["RoleID"] = new SelectList(_context.Set<Role>(), "Name", "Name");
            var inventars = _context.Inventar.Where(i => i.ID == id).ToList();
            ViewData["Inventar"] = inventars;

            return View("AddUser");
        }


        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,Email,Name,EmployeeID,InventarID,RoleID")] User user)
        {
            if (ModelState.IsValid)
            {               
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Users","Inventars", new { id = user.InventarID });
            }

            ModelState.AddModelError("", "A aparut o eroare la validarea datelor introduse.");


            return RedirectToAction("Users", "Inventars", new { id = user.InventarID });
        }




        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string username, int id)
        {
            if (username == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(username, id);

            if (user == null)
            {
                return NotFound();
            }

            ViewData["RoleID"] = new SelectList(_context.Set<Role>(), "Name", "Name", user.RoleID);
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string username, int id, [Bind("Username,Email,Name,EmployeeID,InventarID,RoleID")] User user)
        {
            id = user.InventarID;
            if (username != user.Username)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(username, id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Users", "Inventars", new { id = user.InventarID });
            }

            return View(user);
        }




        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string username, int id)
        {
            if (username == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Inventar)
                .FirstOrDefaultAsync(m => m.Username == username && m.InventarID == id);

            if (user == null)
            {
                return NotFound();
            }

            return View("Delete", user);
        }

        // POST: Users/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete([Bind("Username,Email,Name,EmployeeID,InventarID,RoleID")] User userX)
        {
            var user = await _context.Users.FindAsync(userX.Username, userX.InventarID);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Users", "Inventars", new { id = user.InventarID });
        }

        private bool UserExists(string username, int inventarId)
        {
            return _context.Users.Any(e => e.Username == username && e.InventarID == inventarId);
        }
    }
}