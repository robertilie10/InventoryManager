using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FixedAssetsStockTaking.Data;
using FixedAssetsStockTaking.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Humanizer;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using System.IO;
using OfficeOpenXml;
using System.Security.Policy;
using Newtonsoft.Json;
using static FixedAssetsStockTaking.ViewModels.UserVM;
using FixedAssetsStockTaking.Middleware;

namespace FixedAssetsStockTaking.Controllers
{
    public class InventarsController : Controller
    {
        private readonly FixedAssetsStockTakingContext _context;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        public InventarsController(FixedAssetsStockTakingContext context /*IHttpContextAccessor httpContextAccessor*/)
        {
            _context = context;

            

        }

        // GET: Inventars
        public async Task<IActionResult> Index()
        {

            ////////////////////////////////////////////////////////////
            //var User = HttpContext.User.Identity.Name.Replace("MMRMAKITA\\", "");
            var User = CustomAuth.GetUserName(HttpContext);

            var Superadmins = _context.SuperAdmin.ToList();
            bool isSuperAdmin = Superadmins.Any(admin => admin.Name == User);
            if (!isSuperAdmin)
                ViewData["verify"] = "NO";
            else
                ViewData["verify"] = "YES";
            //////////////////////////////////////////////////////////

            var inventar = _context.Inventar;

            if (inventar != null)
            {
                var inventarSorted = await inventar
                    .OrderBy(i => i.Status == "OPEN" ? 0 : 1)
                    .ThenByDescending(i => i.CreateDate)
                    .ToListAsync();
                var superi = _context.SuperAdmin.ToList();
                ViewData["sups"] = superi;
                ViewData["Inventar"] = inventarSorted;

                return View();
            }

            return Problem("Entity set 'FixedAssetsStockTakingContext.Inventar' is null.");
        }

        // GET: Inventars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Inventar == null)
            {
                return NotFound();
            }

            var inventar = await _context.Inventar
                .FirstOrDefaultAsync(m => m.ID == id);

            //var userPC = _httpContextAccessor.HttpContext.User.Identity.Name;//  HttpContext.User.Identity.Name;
            var userPC = CustomAuth.GetUserName(HttpContext);
            var useri = await _context.Users.Where(u => u.InventarID == id && u.Username == userPC).ToListAsync();
            var superi = await _context.SuperAdmin.Where(s => s.Name == userPC).ToListAsync();
            foreach (var user in useri)
            {
                var ok = 0;
                if (superi.Count != 0)
                {
                    user.OrderRank = 1;
                    ok = 1;
                }
                if (ok == 0)
                {
                    if (user.RoleID == "Admin")
                        user.OrderRank = 1;
                    else
                        user.OrderRank = 0;
                }
                          
            }

           

            if (inventar == null)
            {
                return NotFound();
            }
            ViewData["Superadmins"] = superi;
            ViewData["Users"] = useri;
            return View(inventar);
        }

        //GET: Inventars/Create
        public async Task<IActionResult> CreateUser()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        // POST: Inventars/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(/*[Bind("ID,Name,CreatedBy,ClosedBy,CreateDate,ClosedDate,Status,Comment")]*/ Inventar inventar)
        {
            if (inventar.Name == null)
            {
                return RedirectToAction(nameof(Index));
            }
            //////////////////////////////////////////////////////////
            var User = CustomAuth.GetUserName(HttpContext); //Environment.UserName;
            var Superadmins = _context.SuperAdmin.ToList();
            bool isSuperAdmin = Superadmins.Any(admin => admin.Name == User);
            if (!isSuperAdmin)
            {
                return RedirectToAction("Restricted", "Inventars");
            }
            //////////////////////////////////////////////////////////

            inventar.CreatedBy = CustomAuth.GetUserName(HttpContext); //Environment.UserName;
            inventar.ClosedBy = "Not closed yet";
            inventar.ClosedDate = DateTime.MinValue;
            inventar.CreateDate = DateTime.Now;
            inventar.Status = "OPEN";

            _context.Add(inventar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }



        //[Authorize(Policy = "Admin")]
        // GET: Inventars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //////////////////////////////////////////////////////////
            //var User = Environment.UserName;
            //var Admins = _context.Users.ToList();
            //bool isAdmin = Admins.Any(admin => admin.Username == User && admin.InventarID == id);
            //if (!isAdmin)
            //{
            //    return RedirectToAction("Restricted", "Inventars");
            //}
            //////////////////////////////////////////////////////////
            if (id == null || _context.Inventar == null)
            {
                return NotFound();
            }

            var inventar = await _context.Inventar.FindAsync(id);
            if (inventar == null)
            {
                return NotFound();
            }

            inventar.CreateDate = inventar.CreateDate;
            inventar.ClosedDate = inventar.ClosedDate;

            return View(inventar);
        }




        //[Authorize(Roles ="SuperAdmin")]
        // POST: Inventars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,CreatedBy,ClosedBy,CreateDate,ClosedDate,Status,Comment")] Inventar inventar)
        {

            if (id != inventar.ID)
            {
                return NotFound();
            }

            inventar.Status = "CLOSED";
            inventar.ClosedBy = CustomAuth.GetUserName(HttpContext); //Environment.UserName;
            inventar.ClosedDate = DateTime.Now;

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventarExists(inventar.ID))
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
            return View(inventar);
        }

        // GET: Inventars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null || _context.Inventar == null)
            {
                return NotFound();
            }

            var inventar = await _context.Inventar
                .FirstOrDefaultAsync(m => m.ID == id);
            if (inventar == null)
            {
                return NotFound();
            }

            return View(inventar);
        }

        // POST: Inventars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            if (_context.Inventar == null)
            {
                return Problem("Entity set 'FixedAssetsStockTakingContext.Inventar'  is null.");
            }
            var inventar = await _context.Inventar.FindAsync(id);
            if (inventar != null)
            {
                _context.Inventar.Remove(inventar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventarExists(int id)
        {
            return (_context.Inventar?.Any(e => e.ID == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Start(int? id)
        {
            var inventars = _context.Inventar.Where(u => u.ID == id).ToList();

            ViewData["Inventar"] = inventars;

            return View();

        }


        //private List<MijlocFix> mijlocFixList = new List<MijlocFix>();

        public async Task<IActionResult> FilterRows(string value, string line, int id)
        {   
            
   

            line = line.ToUpper();
            List<MijlocFix> MijloaceLinie = new List<MijlocFix>();

            MijlocFix ScannedItem;

            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(line))
            {
                return BadRequest();
            }
            var c = 0;
            if (line.Length == 3)
            {      
            var parts = value.Split('|');
            if (parts.Length > 0)
            {
                value = parts[0].Trim();
            }
            var ok = 0;
            var mijloc = await _context.MijlocFix.Where(m => m.InventarID == id && 
            ((m.Status == "-" && m.Line == line && (m.FoundLine == line || m.FoundLine == "-")) || (m.FoundLine == line && m.Status == "-"))).ToListAsync();
                foreach (var mij in mijloc)
                {
                    if (mij != null && mij.Locked == "YES")
                    {
                        c++;
                    }
                }
                if (c == mijloc.Count)
                    ok = 1;

            if (ok == 0)
            {
                ScannedItem = await _context.MijlocFix.Where(mf => mf.InventarID == id && mf.COD == value).FirstOrDefaultAsync();

                if (ScannedItem != null)
                {

                    ScannedItem.FoundLine = line;

                        ScannedItem.FoundBy = CustomAuth.GetUserName(HttpContext);// Environment.UserName

                        ScannedItem.FoundAt = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");

                    ScannedItem.FoundQ = 1;

                    if (ScannedItem.Line != ScannedItem.FoundLine)
                        ScannedItem.Observations = "Found on line " + ScannedItem.FoundLine;
                    else
                        ScannedItem.Observations = "";

                    _context.Update(ScannedItem);

                    _context.SaveChanges();
                }
            }
            MijloaceLinie = await _context.MijlocFix
            .Where(r => r.InventarID == id && r.Inventar != null && r.Inventar.Status == "OPEN" && (r.FoundLine == line || r.Line == line))
            .ToListAsync();

            foreach (var item in MijloaceLinie)
            {
                if (item.FoundLine == item.Line)
                {
                    item.DisplayComment = "Found";
                    item.OrderRank = 2;
                }
                else
                {
                    if (item.FoundLine == line)
                    {
                        item.DisplayComment = "Found extra from " + item.Line;
                        item.OrderRank = 3;
                    }
                    else
                    {
                        if (item.FoundLine == "-" || string.IsNullOrWhiteSpace(item.FoundLine))
                        {
                            if (item.Status == "-")
                            {
                                item.DisplayComment = "To find";
                                item.OrderRank = 0;
                            }
                            else
                            {
                                item.DisplayComment = "Not found";
                                item.OrderRank = 1;
                            }
                        }
                        else
                        {
                            item.DisplayComment = "Found on line " + item.FoundLine;
                            item.OrderRank = 4;
                        }
                    }
                }
            }

            MijloaceLinie = MijloaceLinie.OrderBy(r => r.OrderRank).ThenBy(x => x.Line).ToList();
            }
            return PartialView("_FilteredRowsPartial", MijloaceLinie);
        }


        public async Task<IActionResult> RowDiv(string value, int id)
        {
            value = value.ToUpper();
            string ReturnValue="";

            if (string.IsNullOrEmpty(value) || value.Length != 3)
            {
                return BadRequest();
            }
            
            if(_context.MijlocFix.Any(x => x.Line == value && x.InventarID == id))
            {
                ReturnValue = value;
            }
            else
            {
                if (_context.ExtraLines.Any(e => e.MFLine == value && e.InventarID == id))
                {
                    ReturnValue = value;
                }
            }
            /////////////////////////////////////////////////////////////////////////////////
            //var mij = _context.MijlocFix.Where(u => u.InventarID == id).ToList();
            //ViewData["MijlocFix"] = mij;
            /////////////////////////////////////////////////////////////////////////////////
            return PartialView("_RowDivPartial", ReturnValue);
        }

        public async Task<IActionResult> photoLock(string value, int id)
        {
            value = value.ToUpper();
            string ReturnValue = "";
            if (string.IsNullOrEmpty(value) || value.Length != 3)
            {
                return BadRequest();
            }
            var mij = await _context.MijlocFix.Where(m => m.InventarID == id && 
            ((m.FoundLine == "-" && m.Locked == "YES" && m.Line == value) || 
            (m.FoundLine == value && m.Line == value && m.Locked == "YES") ||
            (m.FoundLine == value && m.Locked == "YES"))).ToListAsync();
                if (mij.Count != 0)
                    ReturnValue = "locked";
                else
                    ReturnValue = "unlocked";
            return PartialView("_photoLockPartial", ReturnValue);
        }
        public async Task<IActionResult> Calculus(string value, int id)
        {
            value = value.ToUpper();
            if (string.IsNullOrEmpty(value))
            {
                return BadRequest();
            }
            List<MijlocFix> MijloaceLinie = new List<MijlocFix>();
            MijloaceLinie = await _context.MijlocFix
            .Where(r => r.InventarID == id && r.Inventar != null && r.Inventar.Status == "OPEN" && (r.FoundLine == value || r.Line == value))
            .ToListAsync();

            foreach (var item in MijloaceLinie)
            {
                if (item.FoundLine == item.Line)
                {
                    item.DisplayComment = "Found";
                    item.OrderRank = 1;
                }
                else
                {
                    if (item.FoundLine == value)
                    {
                        item.DisplayComment = "Found extra from " + item.Line;
                        item.OrderRank = 3;
                    }
                    else
                    {
                        if (item.FoundLine == "-" || string.IsNullOrWhiteSpace(item.FoundLine))
                        {
                            item.DisplayComment = "To find";
                            item.OrderRank = 0;
                        }
                        else
                        {
                            item.DisplayComment = "Found on line " + item.FoundLine;
                            item.OrderRank = 2;
                        }
                    }
                }
            }

            MijloaceLinie = MijloaceLinie.OrderBy(r => r.OrderRank).ThenBy(x => x.Line).ToList();

            string serializedData = JsonConvert.SerializeObject(MijloaceLinie);

            return Content(serializedData, "application/json");
        }


        public async Task<IActionResult> EditMijloc( string line, int? id)
        {
            if(line == null)
            {
                return RedirectToAction("Inventory", "Inventars", id);
            }

            List<MijlocFix> MijloaceLinieSpecial = new List<MijlocFix>();
            MijloaceLinieSpecial = await _context.MijlocFix
            .Where(r => r.InventarID == id && r.Inventar != null && r.Inventar.Status == "OPEN" && ((r.FoundLine == line && r.Line != line)||(r.FoundLine=="-" && r.Line==line)))
            .ToListAsync();

            var toate = await _context.MijlocFix
            .Where(r => r.InventarID == id && r.Inventar != null && r.Inventar.Status == "OPEN" && (r.FoundLine == line && r.Line == line))
            .ToListAsync();
            if (MijloaceLinieSpecial.Count > 0)
            {
                foreach(var item in MijloaceLinieSpecial)
                {
                    item.Status = "To check";
                    item.Locked = "YES";
                }
                foreach (var item in toate)
                {
                    item.Locked = "YES";
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Inventory", "Inventars", id);
            }
            else
            {
                foreach (var item in toate)
                {
                    item.Locked = "YES";
                }
                var matchingElements = _context.MijlocFix
               .Where(r => r.InventarID == id && r.Inventar.Status == "OPEN" && r.FoundLine == line && r.Line == line)
               .OrderBy(r => r.FoundAt)
               .ToList();
                var random = new Random();
                var randomElements = matchingElements.OrderBy(x => random.Next()).Take(2).ToList();

                foreach(var item in randomElements)
                {
                    item.Status = "To check";
                }

                ViewData["Random"] = randomElements;
                await _context.SaveChangesAsync();
                return RedirectToAction("Inventory", "Inventars", id);
            }
        }


        public IActionResult Inventory(int? id)
        {
            var inventars = _context.Inventar.Where(u => u.ID == id).ToList();

            var mij = _context.MijlocFix.Where(u => u.InventarID == id).ToList();
            ViewData["MijlocFix"] = mij;

            ViewData["Inventar"] = inventars;
            

            return View();
        }

        public async Task<IActionResult> Line(int? id, string line)
        {
            /////////VERIFY//////////
            var result = VERIFY(id);
            if (result != null)
            {
                return result;
            }
            /////////VERIFY//////////

            List<MijlocFix> test = new List<MijlocFix>();
            test = _context.MijlocFix
            .Where(r => r.InventarID == id && r.Inventar.Status == "OPEN" && 
            ((r.FoundLine == line && r.FoundLine != r.Line) || (r.FoundLine == "-" && r.Line == line) || (r.FoundLine == line && r.Line == line)) &&
            r.Status == "To check")
            .OrderBy(r => r.FoundAt)
            .ToList();

            var inventar = await _context.Inventar
                .FirstOrDefaultAsync(m => m.ID == id);

            var toate = await _context.MijlocFix
            .Where(r => r.InventarID == id && r.Inventar != null && r.Inventar.Status == "OPEN" &&
           ((r.FoundLine == line && r.FoundLine != r.Line) || (r.FoundLine == "-" && r.Line == line) || (r.FoundLine == line && r.Line == line))).ToListAsync();
            ViewData["toate"] = toate;
            ViewData["Inventar"] = inventar;
            ViewData["test"] = test;
            
            return PartialView("_LinePartial", inventar);
        }

        public async Task<IActionResult> UpdateStatus(int id, List<string> cods, string line)
        {
            /////////VERIFY//////////
            var result = VERIFY(id);
            if (result != null)
            {
                return result;
            }
            /////////VERIFY//////////
            var i = 0;
            foreach (var cod in cods)
            {
                var mijlocFix = _context.MijlocFix.FirstOrDefault(m => m.InventarID == id && m.COD == cod);
                if (mijlocFix.FoundLine == mijlocFix.Line && mijlocFix.Status == "To check")
                {
                    mijlocFix.Status = "Checked";
                    mijlocFix.VerifiedBy = CustomAuth.GetUserName(HttpContext);//Environment.UserName;
                    mijlocFix.VerifiedAt = DateTime.Now.ToString();
                }
                if(mijlocFix.FoundLine == "-" && mijlocFix.Status == "To check")
                {
                    mijlocFix.FoundLine = mijlocFix.Line;
                    mijlocFix.FoundBy = CustomAuth.GetUserName(HttpContext);//Environment.UserName;
                    mijlocFix.FoundAt = DateTime.Now.ToString();
                    mijlocFix.Status = "Checked";
                    mijlocFix.VerifiedBy = CustomAuth.GetUserName(HttpContext); //Environment.UserName;
                    mijlocFix.VerifiedAt = DateTime.Now.ToString();
                    mijlocFix.FoundQ = 1;
                }
                if (mijlocFix.FoundLine != mijlocFix.Line && mijlocFix.Status == "To check")
                {
                    mijlocFix.VerifiedBy = CustomAuth.GetUserName(HttpContext); //Environment.UserName;
                    mijlocFix.VerifiedAt = DateTime.Now.ToString();
                    mijlocFix.Status = "Checked";
                    mijlocFix.FoundQ = 1;
                }
            }
                _context.SaveChanges();

            List<MijlocFix> test = new List<MijlocFix>();
            test = _context.MijlocFix
          .Where(r => r.InventarID == id && r.Inventar.Status == "OPEN" &&
          ((r.FoundLine == line && r.FoundLine != r.Line) || (r.FoundLine == "-" && r.Line == line) || (r.FoundLine == line && r.Line == line)) &&
          r.Status == "To check")
          .OrderBy(r => r.FoundAt)
          .ToList();
            var inventar = await _context.Inventar
                .FirstOrDefaultAsync(m => m.ID == id);

            var toate = await _context.MijlocFix
            .Where(r => r.InventarID == id && r.Inventar != null && r.Inventar.Status == "OPEN" &&
           ((r.FoundLine == line && r.FoundLine != r.Line) || (r.FoundLine == "-" && r.Line == line) || (r.FoundLine == line && r.Line == line))).ToListAsync();
            ViewData["toate"] = toate;
            ViewData["Inventar"] = inventar;
            ViewData["test"] = test;

            return PartialView("_LinePartial", inventar);
        }

        public async Task<IActionResult> Scrap(int id, List<string> cods, string line)
        {
            /////////VERIFY//////////
            var result = VERIFY(id);
            if (result != null)
            {
                return result;
            }
            /////////VERIFY//////////

            foreach (var cod in cods)
            {
                var mijlocFix = _context.MijlocFix.FirstOrDefault(m => m.InventarID == id && m.COD == cod);
                if (mijlocFix != null)
                {
                    mijlocFix.FoundLine = "SCR";
                }
            }

            var matchingElements = _context.MijlocFix
              .Where(r => r.InventarID == id && r.FoundLine == line && r.Line == line)
              .OrderBy(r => r.FoundAt)
              .ToList();
            var cont = 0;
            foreach (var element in matchingElements)
            {
                if (element.Status == "-")
                    cont++;
            }
            if (matchingElements.Count == cont)
            {
                var random = new Random();
                var randomElements = matchingElements.OrderBy(x => random.Next()).Take(2).ToList();
                foreach (var element in randomElements)
                {
                    element.Status = "Checked";
                }
            }

            _context.SaveChanges();

            List<MijlocFix> test = new List<MijlocFix>();
            test = _context.MijlocFix
          .Where(r => r.InventarID == id && r.Inventar.Status == "OPEN" &&
          ((r.FoundLine == line && r.FoundLine != r.Line) || (r.FoundLine == "-" && r.Line == line) || (r.FoundLine == line && r.Line == line)) &&
          r.Status == "To check")
          .OrderBy(r => r.FoundAt)
          .ToList();
            var inventar = await _context.Inventar
                .FirstOrDefaultAsync(m => m.ID == id);

            var toate = await _context.MijlocFix
            .Where(r => r.InventarID == id && r.Inventar != null && r.Inventar.Status == "OPEN" &&
           ((r.FoundLine == line && r.FoundLine != r.Line) || (r.FoundLine == "-" && r.Line == line) || (r.FoundLine == line && r.Line == line))).ToListAsync();
            ViewData["toate"] = toate;
            ViewData["Inventar"] = inventar;
            ViewData["test"] = test;

            return PartialView("_LinePartial", inventar);
        }


        public async Task<IActionResult> LockUnlock(int id, string line)
        {

            /////////VERIFY//////////
            var result = VERIFY(id);
            if (result != null)
            {
                return result;
            }
            /////////VERIFY//////////

            List<MijlocFix> mij = new List<MijlocFix>();
            mij = _context.MijlocFix
            .Where(r => r.InventarID == id && r.Inventar != null && r.Inventar.Status == "OPEN" &&
           ((r.FoundLine == line && r.FoundLine != r.Line) || (r.FoundLine == "-" && r.Line == line) || (r.FoundLine == line && r.Line == line)))
            .ToList();
            foreach(var mijloc in mij)
            {
                if (mijloc.Locked == "YES")             
                   mijloc.Locked = "NO";
                 else
                   mijloc.Locked = "YES";
                
                    if (mijloc.Status == "Checked")
                    {
                        mijloc.Status = "To check";
                        mijloc.VerifiedBy = "-";
                        mijloc.VerifiedAt = "-";
                    }
                    
                
                
            }
            _context.SaveChanges();

            var inventar =await _context.Inventar
                .FirstOrDefaultAsync(m => m.ID == id);

            var toate = await _context.MijlocFix
            .Where(r => r.InventarID == id && r.Inventar != null && r.Inventar.Status == "OPEN" &&
           ((r.FoundLine == line && r.FoundLine != r.Line) || (r.FoundLine == "-" && r.Line == line) || (r.FoundLine == line && r.Line == line))).ToListAsync();
            ViewData["toate"] = toate;
            ViewData["Inventar"] = inventar;
            ViewData["test"] = mij;
            return PartialView("_LinePartial",inventar);
        }

        private IActionResult VERIFY(int? id)
        {
            var User = CustomAuth.GetUserName(HttpContext); //Environment.UserName;
            var Superadmins = _context.SuperAdmin.ToList();
            var Admins = _context.Users.Where(u => u.InventarID == id && u.RoleID == "Admin");
            bool isAdmin = Admins.Any(a => a.Username == User);
            bool isSuperAdmin = Superadmins.Any(admin => admin.Name == User);
            if (!isAdmin)
            {
                if (!isSuperAdmin)
                    return RedirectToAction("Restricted", "Inventars");
            }
            return null;
        }

        public async Task<IActionResult> Supervisor(int ?id)
        {
            /////////VERIFY//////////
            var result = VERIFY(id);
            if (result != null)
            {
                return result;
            }
            /////////VERIFY//////////
            var inventar = await _context.Inventar
                .FirstOrDefaultAsync(m => m.ID == id);
            var inventars = _context.Inventar.Where(u => u.ID == id).ToList();

            var mij = _context.MijlocFix.Where(m => m.ID == id).ToList();

            List<MijlocFix> MijloaceLinie = new List<MijlocFix>();
            MijloaceLinie = _context.MijlocFix
            .Where(r => r.InventarID == id && r.Inventar.Status == "OPEN")
            .ToList();

            List<ExtraLine> extra = new List<ExtraLine>();
            extra = _context.ExtraLines.Where(e => e.InventarID == id).ToList();

            ViewData["ExtraLines"] = extra;
            ViewData["MijlocFix"] = MijloaceLinie;
            ViewData["Inventar"] = inventars;

            return View(inventar);
        }

        public IActionResult Restricted(int? id)
        {
            var inventars = _context.Inventar.Where(u => u.ID == id).ToList();

            ViewData["Inventar"] = inventars;

            return View();
        }

        public async Task<IActionResult> RevenueCenters(int? id)
        {

            /////////VERIFY//////////
            var result = VERIFY(id);
            if (result != null)
            {
                return result;
            }
            /////////VERIFY//////////
            
            if (id == null || _context.Inventar == null)
            {
                return NotFound();
            }

            var inventar = _context.Inventar
                .Where(m => m.ID == id).FirstOrDefault();

            var mij = _context.MijlocFix.Where(m => m.ID == id).ToList();
            if (inventar == null)
            {
                return NotFound();
            }

            List<MijlocFix> MijloaceLinie = new List<MijlocFix>();

            MijloaceLinie = _context.MijlocFix
            .Where(r => r.InventarID == id && r.Inventar.Status == "OPEN")
            .ToList();

            var items = _context.MijlocFix.Where(mf => mf.InventarID == id).ToList();

            var extra = _context.ExtraLines.Where(e => e.InventarID == id).ToList();

            ViewData["ExtraLines"] = extra;
            ViewData["MijlocFix"] = items;
            ViewData["MijlocFix2"] = MijloaceLinie;
            ViewData["Inventar"] = inventar;


            return View(inventar);
        }

        public async Task<IActionResult> Settings(int? id)
        {
            /////////VERIFY//////////
            var result = VERIFY(id);
            if (result != null)
            {
                return result;
            }
            /////////VERIFY//////////

            var inventars =await _context.Inventar.Where(u => u.ID == id).FirstOrDefaultAsync();

            ViewData["Inventar"] = inventars;

            return View(inventars);
        }


        public async Task<IActionResult> Users(int? id)
        {
            /////////VERIFY//////////
            var result = VERIFY(id);
            if (result != null)
            {
                return result;
            }
            /////////VERIFY//////////

            if (id == null || _context.Inventar == null)
            {
                return NotFound();
            }

            var inventar = await _context.Inventar
                .FirstOrDefaultAsync(m => m.ID == id);
            if (inventar == null)
            {
                return NotFound();
            }
            var users = _context.Users.Where(u => u.InventarID == id).OrderBy(u => u.RoleID).ToList();

            ViewData["User"] = users;

            var inventars = _context.Inventar.Where(u => u.ID == id).ToList();

            ViewData["Inventar"] = inventars;

            return View(inventar);
        }

        public async Task<IActionResult> Files(int? id)
        {
            /////////VERIFY//////////
            var result = VERIFY(id);
            if (result != null)
            {
                return result;
            }
            /////////VERIFY//////////

            var inventars = _context.Inventar.Where(u => u.ID == id).ToList();

            var mijloace = _context.MijlocFix.Where(m => m.InventarID == id).ToList();
            if (mijloace.Count == 0)
            {
                ViewData["MijlocFix"] = null;
            }
            else
            {
                ViewData["MijlocFix"] = mijloace;
            }

            ViewData["Inventar"] = inventars;
            ViewData["InventarID"] = inventars[0].ID;

            return View("Files");
        }

        public async Task<IActionResult> Overview(int? id)
        {
            /////////VERIFY//////////
            //var result = VERIFY(id);
            //if (result != null)
            //{
            //    return result;
            //}
            /////////VERIFY//////////

            if (id == null || _context.Inventar == null)
            {
                return NotFound();
            }

            var inventar = _context.Inventar
                .Where(m => m.ID == id).FirstOrDefault();

            
            if (inventar == null)
            {
                return NotFound();
            }
            var mij = _context.MijlocFix.Where(m => m.ID == id).ToList();
            List<MijlocFix> MijloaceLinie = new List<MijlocFix>();
            List<ExtraLine> extra = new List<ExtraLine>();

            MijloaceLinie = _context.MijlocFix
            .Where(r => r.InventarID == id && r.Inventar.Status == "OPEN")
            .ToList();

            var items = _context.MijlocFix.Where(mf => mf.InventarID == id).ToList();

            extra = _context.ExtraLines.Where(e => e.InventarID == id).ToList();

            ViewData["ExtraLines"] = extra;
            ViewData["MijlocFix2"] = MijloaceLinie;
            return View(inventar);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, int id)
        {
            /////////VERIFY//////////
            var result = VERIFY(id);
            if (result != null)
            {
                return result;
            }
            /////////VERIFY//////////

            if (file == null || id == null)
            {
                return NotFound();
            }


            try
            {
                var mijloace = _context.MijlocFix.Where(m => m.InventarID == id).ToList();
                if (mijloace.Count != 0)
                {
                    _context.MijlocFix.RemoveRange(mijloace);
                    _context.SaveChanges();
                }


                int startingLine = 10;
                var Mij = new List<MijlocFix>();
                if (file != null)
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    // Procesați fișierul Excel
                    using (var pacakage = new ExcelPackage(file.OpenReadStream()))
                    {

                        ExcelWorkbook workbook = pacakage.Workbook;

                        int sheetCount = workbook.Worksheets.Count;

                        for (int sheetIndex = 0; sheetIndex < sheetCount; sheetIndex++)
                        {
                            ExcelWorksheet worksheet = workbook.Worksheets[sheetIndex];
                            int rowCount = worksheet.Dimension.Rows;
                            int colCount = worksheet.Dimension.Columns;

                            for (int row = 10; row <= rowCount && worksheet.Cells[row, 5].Value != null; row++)
                            {
                                var entry = new MijlocFix();

                                if (row == 77 && sheetIndex == 3)
                                {
                                    var x = 12;
                                }

                                entry.COD = worksheet.Cells[row, 3].Value?.ToString();
                                entry.Description = worksheet.Cells[row, 4].Value?.ToString();
                                entry.AlocatedCenter = worksheet.Cells[row, 5].Value?.ToString();
                                entry.WrittenQ = Convert.ToInt32(worksheet.Cells[row, 6].Value?.ToString());

                                entry.FoundLine = "-";
                                entry.FoundBy = "-";
                                entry.FoundAt = "-";
                                entry.VerifiedAt = "-";
                                entry.VerifiedBy = "-";
                                entry.Line = " ";
                                entry.Locked = "NO";
                                entry.Status = "-";

                                for (int i = entry.AlocatedCenter.Length; i >= 0 && entry.Line.Length < 4; i--)
                                {
                                    entry.Line = entry.Line + entry.AlocatedCenter[i - 4] + entry.AlocatedCenter[i - 3] + entry.AlocatedCenter[i - 2];
                                    break;
                                }
                                entry.FoundQ = 0;

                                entry.Inventar = await _context.Inventar.Where(i => i.ID == id).FirstOrDefaultAsync();
                                entry.InventarID = id;

                                entry.Line = entry.Line.Trim();

                                Mij.Add(entry);
                            }
                        }
                    }
                }
                _context.MijlocFix.AddRange(Mij);

                var item = await _context.Inventar.Where(i => i.ID == id).FirstOrDefaultAsync();
                _context.SaveChanges();



                return Json(new { message = "ok" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "A apărut o eroare la procesarea cererii." });
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////





        public ActionResult Download(int? id)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Asamblare");

            var data = _context.MijlocFix.Where(m => m.InventarID == id).ToList();

            var inventar = _context.Inventar.Where(m => m.ID == id).FirstOrDefault();
            worksheet.Cells["E1:H1"].Merge = true;

            // Setarea valorii și a stilului pentru celula unificată
            var mergedCell = worksheet.Cells["E1:H1"];
            mergedCell.Value = inventar.Name;
            mergedCell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            mergedCell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            mergedCell.Style.Font.Size = 24;


            int j = 3;
            // Loop through the data and populate the cells of the worksheet
            for (int i = 0; i < data.Count + 1; i++)
            {
                if (i == 0)
                {
                    worksheet.Column(1).Width = 13;
                    worksheet.Column(2).Width = 45;
                    worksheet.Column(3).Width = 15;
                    worksheet.Column(5).Width = 12;
                    worksheet.Column(7).Width = 19;
                    worksheet.Column(8).Width = 13;
                    worksheet.Column(9).Width = 19;
                    worksheet.Column(10).Width = 18;
                    worksheet.Column(11).Width = 15;
                    worksheet.Column(12).Width = 20;
                    worksheet.Cells["A3"].Value = "COD";
                    worksheet.Cells["B3"].Value = "Denumire mijloc fix";
                    worksheet.Cells["C3"].Value = "Centru cost";
                    worksheet.Cells["D3"].Value = "Linie";
                    worksheet.Cells["E3"].Value = "Gasit la linia";
                    worksheet.Cells["F3"].Value = "Gasit de";
                    worksheet.Cells["G3"].Value = "Gasit la";
                    worksheet.Cells["H3"].Value = "Verificat de";
                    worksheet.Cells["I3"].Value = "Verificat la";
                    worksheet.Cells["J3"].Value = "Cantitate scriptica";
                    worksheet.Cells["K3"].Value = "Cantitate gasita";
                    worksheet.Cells["L3"].Value = "Observatii";
                    for (int colIndex = 1; colIndex <= 12; colIndex++)
                    {
                        worksheet.Cells[3, colIndex].Style.Font.Bold = true; // Setarea textului îngroșat pentru prima linie
                        worksheet.Cells[3, colIndex].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }
                }
                else
                {
                    worksheet.Cells["A" + (j + 1)].Value = data[i - 1].COD;
                    worksheet.Cells["B" + (j + 1)].Value = data[i - 1].Description;
                    worksheet.Cells["C" + (j + 1)].Value = data[i - 1].AlocatedCenter;
                    worksheet.Cells["D" + (j + 1)].Value = data[i - 1].Line;
                    worksheet.Cells["E" + (j + 1)].Value = data[i - 1].FoundLine;
                    worksheet.Cells["F" + (j + 1)].Value = data[i - 1].FoundBy;
                    worksheet.Cells["G" + (j + 1)].Value = data[i - 1].FoundAt;
                    worksheet.Cells["H" + (j + 1)].Value = data[i - 1].VerifiedBy;
                    worksheet.Cells["I" + (j + 1)].Value = data[i - 1].VerifiedAt;
                    worksheet.Cells["J" + (j + 1)].Value = data[i - 1].WrittenQ;
                    worksheet.Cells["K" + (j + 1)].Value = data[i - 1].FoundQ;
                    worksheet.Cells["L" + (j + 1)].Value = data[i - 1].Observations;
                    
                    j++;

                    for (int colIndex = 1; colIndex <= 12; colIndex++)
                    {
                        worksheet.Cells[j, colIndex].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }
                }

            }

            var distinctAllUsers = data
     .Select(d => d.FoundBy)
     .Concat(data.Select(d => d.VerifiedBy))
     .Where(user => user != "-")
     .Distinct()
     .ToList();


           
            int userRow = 2042;

            foreach (var user in distinctAllUsers)
            {
                worksheet.Cells["B" + userRow].Value = user;
                worksheet.Cells["B" + userRow].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                userRow++;
            }

            byte[] fileBytes = package.GetAsByteArray();

            // Get the path to the desktop
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            var inventars = _context.Inventar.Where(u => u.ID == id).ToList();

            // Return the file for download
            string fileName = inventars[0].Name + "_" + DateTime.Today.ToString("dd") + "-" + DateTime.Today.ToString("MM") + "-" + DateTime.Today.ToString("yyyy") + ".xlsx";

            string filePath = Path.Combine(desktopPath, fileName);

            // Save the file to the Desktop folder
            System.IO.File.WriteAllBytes(filePath, fileBytes);

            // Return the file for download
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

        }
    }
}

