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
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user != default)
            {
                if (await _userManager.IsInRoleAsync(user, "HR") && user.CompanyId != null)
                {
                    ViewBag.CompanyId = user.CompanyId;
                    return View(await _context.Vacancies.Where(x => x.CompanyId == user.CompanyId).ToListAsync());
                }
            }
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

            VacancyApply vacancyApply = null;
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user != default)
            {
                vacancyApply = await _context.VacancyApplies.Include(x => x.User).FirstOrDefaultAsync(x => x.VacancyId == vacancy.Id && x.User.Id == user.Id);
            }

            var model = new VacancyDetailsViewModel
            {
                Vacancy = vacancy,
                VacancyApply = vacancyApply
            };

            return View(model);
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
                RequiredSkills = vacancy.RequiredSkills,
                TechStack = vacancy.TechStack,
                AdditionalInfo = vacancy.AdditionalInfo,
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
                vacancy.RequiredSkills = model.RequiredSkills;
                vacancy.TechStack = model.TechStack;
                vacancy.AdditionalInfo = model.AdditionalInfo;

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

            VacancyApply vacancyApply = new()
            {
                User = user,
                VacancyId = vacancy.Id,
                Status = VacancyApplyStatus.Pending
            };

            _context.VacancyApplies.Add(vacancyApply);

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
                .Include(v => v.Applicants.Where(x => x.User.ShowInApplicantsList))
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
