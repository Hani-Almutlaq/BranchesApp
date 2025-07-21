using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BranchesApp.Data;
using BranchesApp.Models;

namespace BranchesApp.Controllers
{
    public class ShiftsController : Controller
    {
        private readonly AppDbContext _context;

        public ShiftsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Shifts
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Shifts.Include(s => s.Branch).Include(s => s.Day);

            return View(await appDbContext.ToListAsync());
        }

        // GET: Shifts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var shift = await _context.Shifts.Include(s => s.Branch).Include(s => s.Day).FirstOrDefaultAsync(m => m.ShiftId == id);

            if (shift == null)
                return NotFound();

            return View(shift);
        }

        // GET: Shifts/Create
        public IActionResult Create()
        {
            ViewData["BranchId"] = new SelectList(_context.Branches, "BranchId", "BranchName");
            ViewData["DayId"] = new SelectList(_context.Days, "DayId", "DayName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShiftId,BranchId,DayId,StartTime,EndTime,OpenAllDay")] Shift shift)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["BranchId"] = new SelectList(_context.Branches, "BranchId", "BranchId", shift.BranchId);
            ViewData["DayId"] = new SelectList(_context.Days, "DayId", "DayId", shift.DayId);

            return View(shift);
        }

        // GET: Shifts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var shift = await _context.Shifts.FindAsync(id);

            if (shift == null)
                return NotFound();

            ViewData["BranchId"] = new SelectList(_context.Branches, "BranchId", "BranchName", shift.BranchId);
            ViewData["DayId"] = new SelectList(_context.Days, "DayId", "DayName", shift.DayId);

            return View(shift);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShiftId,BranchId,DayId,StartTime,EndTime,OpenAllDay")] Shift shift)
        {
            if (id != shift.ShiftId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shift);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShiftExists(shift.ShiftId))
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

            ViewData["BranchId"] = new SelectList(_context.Branches, "BranchId", "BranchId", shift.BranchId);
            ViewData["DayId"] = new SelectList(_context.Days, "DayId", "DayId", shift.DayId);

            return View(shift);
        }

        // GET: Shifts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var shift = await _context.Shifts.Include(s => s.Branch).Include(s => s.Day).FirstOrDefaultAsync(m => m.ShiftId == id);

            if (shift == null)
                return NotFound();

            return View(shift);
        }

        // POST: Shifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shift = await _context.Shifts.FindAsync(id);

            if (shift != null)
                _context.Shifts.Remove(shift);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShiftExists(int id)
        {
            return _context.Shifts.Any(e => e.ShiftId == id);
        }
    }
}
