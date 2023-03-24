using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ESD.Data;
using ESD.Models;

namespace ESD.Controllers
{
    public class DepartmentUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DepartmentUsers
        public async Task<IActionResult> Index()
        {
              return _context.DepartmentUsers != null ? 
                          View(await _context.DepartmentUsers.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.DepartmentUsers'  is null.");
        }

        // GET: DepartmentUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DepartmentUsers == null)
            {
                return NotFound();
            }

            var departmentUser = await _context.DepartmentUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (departmentUser == null)
            {
                return NotFound();
            }

            return View(departmentUser);
        }

        // GET: DepartmentUsers/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        // POST: DepartmentUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,DepartmentId")] DepartmentUser departmentUser)
        {
            //if (ModelState.IsValid)
            if (!DepartmentUserExist(departmentUser.DepartmentId, departmentUser.UserId))
            {
                _context.Add(departmentUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: DepartmentUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DepartmentUsers == null)
            {
                return NotFound();
            }

            var departmentUser = await _context.DepartmentUsers.FindAsync(id);
            if (departmentUser == null)
            {
                return NotFound();
            }

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");

            return View(departmentUser);
        }

        // POST: DepartmentUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,DepartmentId")] DepartmentUser departmentUser)
        {
            if (id != departmentUser.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            if (!DepartmentUserExist(departmentUser.DepartmentId, departmentUser.UserId))
            {
                try
                {
                    _context.Update(departmentUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentUserExists(departmentUser.Id))
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
            return View(departmentUser);
        }

        // GET: DepartmentUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DepartmentUsers == null)
            {
                return NotFound();
            }

            var departmentUser = await _context.DepartmentUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (departmentUser == null)
            {
                return NotFound();
            }

            return View(departmentUser);
        }

        // POST: DepartmentUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DepartmentUsers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DepartmentUsers'  is null.");
            }
            var departmentUser = await _context.DepartmentUsers.FindAsync(id);
            if (departmentUser != null)
            {
                _context.DepartmentUsers.Remove(departmentUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentUserExists(int id)
        {
          return (_context.DepartmentUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool DepartmentUserExist(int DepartmentID, string UserID)
        {
            DepartmentUser DUcheck = _context.DepartmentUsers.Where(du => du.DepartmentId == DepartmentID).Where(du => du.UserId == UserID).FirstOrDefault();

            if (DUcheck != null)
            {
                return true;
            }

            return false;

        }
    }
}
