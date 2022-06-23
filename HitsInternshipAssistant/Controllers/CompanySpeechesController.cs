using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HitsInternshipAssistant.Data;
using HitsInternshipAssistant.Data.Models;
using Microsoft.AspNetCore.Authorization;
using HitsInternshipAssistant.Data.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace HitsInternshipAssistant.Controllers
{
    public class CompanySpeechesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompanySpeechesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.companySpeeches.Include(c => c.Company);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> GetByMonth(Month month)
        {
            var applicationDbContext = _context.companySpeeches.Include(c => c.Company).Where(x => x.StartTime.Month == (int)month);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize(Roles = "Admin, University, HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? companyId, CreateCompanySpeechViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (companyId == null)
            {
                if (await _userManager.IsInRoleAsync(user, Roles.HR) && user.CompanyId != null)
                {
                    companyId = user.CompanyId;
                }

                return BadRequest();
            }

            var company = await _context.Companies.FirstOrDefaultAsync(x => x.Id == companyId);
            if (company == default)
            {
                return BadRequest();
            }

            var companySpeech = new CompanySpeech
            {
                CompanyId = (Guid)companyId,
                StartTime = model.StartTime,
                EndTime = model.EndTime
            };

            _context.Add(companySpeech);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.companySpeeches == null)
            {
                return NotFound();
            }

            var companySpeech = await _context.companySpeeches
                .Include(c => c.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companySpeech == null)
            {
                return NotFound();
            }

            return View(companySpeech);
        }

        // POST: CompanySpeeches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.companySpeeches == null)
            {
                return Problem("Entity set 'ApplicationDbContext.companySpeeches'  is null.");
            }
            var companySpeech = await _context.companySpeeches.FindAsync(id);
            if (companySpeech != null)
            {
                _context.companySpeeches.Remove(companySpeech);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanySpeechExists(Guid id)
        {
            return (_context.companySpeeches?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
