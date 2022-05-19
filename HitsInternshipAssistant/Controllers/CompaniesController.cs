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
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CompaniesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Companies.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .Include(x => x.Vacancies)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            var interns = await _context.Users
                .Where(x => x.CompanyId == company.Id &&
                            !Task.Run(() => _userManager.IsInRoleAsync(x, Roles.HR)).Result)
                .ToListAsync();

            var model = new CompanyDetailsViewModel
            {
                Company = company,
                Interns = interns
            };

            return View(model);
        }

        [Authorize(Roles = "Admin, University, HR")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin, University, HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                bool isCurrentUserHR = User.IsInRole(Roles.HR);

                Company company = new()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Color = model.Color
                };

                if (isCurrentUserHR)
                {
                    user.CompanyId = company.Id;
                }

                _context.Add(company);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [Authorize(Roles = "Admin, University, HR")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        [Authorize(Roles = "Admin, University, HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var company = await GetCompanyAsync(id);
                if (company == default)
                {
                    return NotFound();
                }

                company.Name = model.Name;
                company.Description = model.Description;
                company.Color = model.Color;

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

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        [Authorize(Roles = "Admin, University, HR")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var company = await _context.Companies.FindAsync(id);
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin, University, HR")]
        public async Task<IActionResult> CreateVacancy(Guid? companyId)
        {
            Company company = await GetCompanyAsync(companyId);
            if (company == default)
            {
                return NotFound();
            }

            return View(new CreateVacancyViewModel());
        }

        [Authorize(Roles = "Admin, University, HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVacancy(Guid companyId, CreateVacancyViewModel model)
        {
            if (ModelState.IsValid)
            {
                Company company = await GetCompanyAsync(companyId);
                if (company == default)
                {
                    return NotFound();
                }

                Vacancy vacancy = new()
                {
                    Name = model.Name,
                    Description = model.Description,
                    CompanyId = company.Id
                };

                _context.Add(vacancy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        private bool CompanyExists(Guid id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }

        private async Task<Company> GetCompanyAsync(Guid? id)
        {
            return await _context.Companies.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
