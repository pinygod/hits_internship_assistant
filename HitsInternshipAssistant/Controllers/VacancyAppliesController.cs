#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HitsInternshipAssistant.Data;
using HitsInternshipAssistant.Data.Models;
using Microsoft.AspNetCore.Authorization;
using HitsInternshipAssistant.Data.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace HitsInternshipAssistant.Controllers
{
    public class VacancyAppliesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VacancyAppliesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            if (User.IsInRole(Roles.Admin) || User.IsInRole(Roles.University))
            {
                return View(await _context.VacancyApplies
                    .Include(x => x.Vacancy)
                    .Include(x => x.User)
                    .ToListAsync());
            }
            else if (User.IsInRole(Roles.HR))
            {
                return View(await _context.VacancyApplies
                    .Include(x => x.Vacancy)
                    .Include(x => x.User)
                    .Where(x => x.Vacancy.CompanyId == user.CompanyId)
                    .ToListAsync());
            }
            else
            {
                return View(await _context.VacancyApplies
                    .Include(x => x.Vacancy)
                    .Include(x => x.User)
                    .Where(x => x.User.Id == user.Id)
                    .ToListAsync());
            }
        }

        [Authorize]
        public async Task<IActionResult> Details(Guid? id)
        {
            VacancyApply vacancyApply = await _context.VacancyApplies
                .Include(x => x.Vacancy)
                .Include(x => x.User)
                .ThenInclude(x => x.CV)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (vacancyApply == default)
            {
                return NotFound();
            }

            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user.Id != vacancyApply.User.Id ||
                !User.IsInRole(Roles.Admin) ||
                !User.IsInRole(Roles.University) ||
                !User.IsInRole(Roles.HR))
            {
                return Forbid();
            }

            return View(vacancyApply);
        }

        [Authorize(Roles = "Admin, Unversity, HR")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            VacancyApply vacancyApply = await _context.VacancyApplies.FirstOrDefaultAsync(x => x.Id == id);
            if (vacancyApply == default)
            {
                return NotFound();
            }

            return View();
        }

        [Authorize(Roles = "Admin, Unversity, HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditVacancyApplyViewModel model)
        {
            if (ModelState.IsValid)
            {
                VacancyApply vacancyApply = await _context.VacancyApplies.FirstOrDefaultAsync(x => x.Id == id);
                if (vacancyApply == default)
                {
                    return NotFound();
                }

                vacancyApply.Status = model.Status;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Delete(Guid? id)
        {
            VacancyApply vacancyApply = await _context.VacancyApplies.FirstOrDefaultAsync(x => x.Id == id);
            if (vacancyApply == default)
            {
                return NotFound();
            }

            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user.Id != vacancyApply.User.Id.ToString() ||
                !User.IsInRole(Roles.Admin) ||
                !User.IsInRole(Roles.University))
            {
                return Forbid();
            }

            return View(vacancyApply);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            VacancyApply vacancyApply = await _context.VacancyApplies.FirstOrDefaultAsync(x => x.Id == id);
            if (vacancyApply == default)
            {
                return NotFound();
            }

            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user.Id != vacancyApply.User.Id.ToString() ||
                !User.IsInRole(Roles.Admin) ||
                !User.IsInRole(Roles.University))
            {
                return Forbid();
            }

            _context.VacancyApplies.Remove(vacancyApply);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
