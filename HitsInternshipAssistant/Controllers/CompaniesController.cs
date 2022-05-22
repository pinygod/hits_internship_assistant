#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HitsInternshipAssistant.Data;
using HitsInternshipAssistant.Data.Models;
using Microsoft.AspNetCore.Authorization;
using HitsInternshipAssistant.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using HitsInternshipAssistant.Services;

namespace HitsInternshipAssistant.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly FileUploadsService _fileUploadsService;

        public CompaniesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, FileUploadsService imageUploadService)
        {
            _context = context;
            _userManager = userManager;
            _fileUploadsService = imageUploadService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Companies.ToListAsync());
        }

        [Authorize(Roles = "Admin, University, HR")]
        public async Task<List<ApplicationUser>> GetInterns(Guid? companyId)
        {
            if (companyId == null)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                companyId = user.CompanyId;
            }

            return await _context.Users.Where(x => x.CompanyId == companyId && !Task.Run(() => _userManager.IsInRoleAsync(x, "HR")).Result).ToListAsync();
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .Include(x=> x.Employees)
                .Include(x => x.Vacancies)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            var interns = await _context.Users
                .Where(x => x.CompanyId == company.Id &&
                            x.ShowInInternsList &&
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
            string logoPath = "", backgroundImagePath = "";
            try
            {
                logoPath = await _fileUploadsService.UploadImageAsync(model.Logo);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(model.Logo), ex.Message);
            }
            try
            {
                backgroundImagePath = await _fileUploadsService.UploadImageAsync(model.BackgroundLogo);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(model.BackgroundLogo), ex.Message);
            }

            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                bool isCurrentUserHR = User.IsInRole(Roles.HR);

                Company company = new()
                {
                    Name = model.Name,
                    ShortName = model.ShortName,
                    Tagline = model.Tagline,
                    Description = model.Description,
                    Contacts = model.Contacts,
                    PartnershipStartYear = model.PartnershipStartYear,
                    LogoLink = logoPath,
                    BackgroundLogoLink = backgroundImagePath,
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
            string logoPath = null, backgroundImagePath = null;
            if (model.Logo != null)
            {
                try
                {
                    logoPath = await _fileUploadsService.UploadImageAsync(model.Logo);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(model.Logo), ex.Message);
                }
            }

            if (model.BackgroundLogo != null)
            {
                try
                {
                    backgroundImagePath = await _fileUploadsService.UploadImageAsync(model.BackgroundLogo);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(model.BackgroundLogo), ex.Message);
                }
            }

            if (ModelState.IsValid)
            {
                var company = await GetCompanyAsync(id);
                if (company == default)
                {
                    return NotFound();
                }

                company.Name = model.Name;
                company.ShortName = model.ShortName;
                company.Tagline = model.Tagline;
                company.Description = model.Description;
                company.Contacts = model.Contacts;
                company.PartnershipStartYear = model.PartnershipStartYear;
                if (logoPath != null)
                {
                    company.LogoLink = logoPath;
                }
                if (backgroundImagePath != null)
                {
                    company.BackgroundLogoLink = backgroundImagePath;
                }

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
                    RequiredSkills = model.RequiredSkills,
                    TechStack = model.TechStack,
                    AdditionalInfo = model.AdditionalInfo,
                    CompanyId = company.Id
                };

                _context.Add(vacancy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [Authorize(Roles = "Admin, University")]
        public async Task<IActionResult> AddEmployee(Guid companyId)
        {
            var company = await _context.Companies.FindAsync(companyId);
            if (company == null)
            {
                return NotFound();
            }

            ViewBag["Users"] = await _context.Users.ToListAsync();

            return View();
        }

        [Authorize(Roles = "Admin, University")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee(Guid companyId, Guid userId)
        {
            var company = await _context.Companies.FindAsync(companyId);
            if (company == null)
            {
                return NotFound();
            }

            ApplicationUser user = await _userManager.FindByIdAsync(userId.ToString());

            user.CompanyId = companyId;
            company.Employees.Add(user);

            await _context.SaveChangesAsync();

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
