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
    public class VacanciesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VacanciesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Vacancies.Include(v => v.Company).ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacancy = await GetVacancyAsync(id);
            if (vacancy == null)
            {
                return NotFound();
            }

            return View(vacancy);
        }

        [Authorize(Roles = "Admin, University, HR")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacancy = await GetVacancyAsync(id);
            if (vacancy == null)
            {
                return NotFound();
            }

            EditVacancyViewModel vacancyViewModel = new()
            {
                Name = vacancy.Name,
                Description = vacancy.Description
            };

            return View(vacancyViewModel);
        }

        [Authorize(Roles = "Admin, University, HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditVacancyViewModel model)
        {
            Vacancy vacancy = await GetVacancyAsync(id);
            if (vacancy == default)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                vacancy.Name = model.Name;
                vacancy.Description = model.Description;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [Authorize(Roles = "Admin, University, HR")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacancy = await GetVacancyAsync(id);
            if (vacancy == null)
            {
                return NotFound();
            }

            return View(vacancy);
        }

        [Authorize(Roles = "Admin, University, HR")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var vacancy = await _context.Vacancies.FindAsync(id);
            _context.Vacancies.Remove(vacancy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpPost]
        public async Task<bool> Apply(Guid vacancyId)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            Vacancy vacancy = await _context.Vacancies.FindAsync(vacancyId);

            vacancy.Applicants.Add(user);
            user.VacanciesApplied.Add(vacancy);

            await _context.SaveChangesAsync();

            return true;
        }

        private bool VacancyExists(Guid id)
        {
            return _context.Vacancies.Any(e => e.Id == id);
        }

        private async Task<Vacancy> GetVacancyAsync(Guid? id)
        {
            return await _context.Vacancies
                .Include(v => v.Company)
                .Include(v => v.Applicants)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
