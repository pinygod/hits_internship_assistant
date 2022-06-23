#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HitsInternshipAssistant.Data;
using HitsInternshipAssistant.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace HitsInternshipAssistant.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class WorkDirectionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkDirectionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.WorkDirections.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workDirection = await _context.WorkDirections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workDirection == null)
            {
                return NotFound();
            }

            return View(workDirection);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] WorkDirection workDirection)
        {
            if (ModelState.IsValid)
            {
                workDirection.Id = Guid.NewGuid();
                _context.Add(workDirection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workDirection);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workDirection = await _context.WorkDirections.FindAsync(id);
            if (workDirection == null)
            {
                return NotFound();
            }
            return View(workDirection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description")] WorkDirection workDirection)
        {
            if (id != workDirection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workDirection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkDirectionExists(workDirection.Id))
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
            return View(workDirection);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workDirection = await _context.WorkDirections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workDirection == null)
            {
                return NotFound();
            }

            return View(workDirection);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var workDirection = await _context.WorkDirections.FindAsync(id);
            _context.WorkDirections.Remove(workDirection);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkDirectionExists(Guid id)
        {
            return _context.WorkDirections.Any(e => e.Id == id);
        }
    }
}
