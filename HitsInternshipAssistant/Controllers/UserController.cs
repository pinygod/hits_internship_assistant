using HitsInternshipAssistant.Data;
using HitsInternshipAssistant.Data.Models;
using HitsInternshipAssistant.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HitsInternshipAssistant.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.Include(x => x.Company).ToListAsync());
        }

        [Authorize(Roles = "Admin, University")]
        public async Task<IActionResult> GetStudents()
        {
            return View(await _context.Users.Include(x => x.Company).Where(x => x.Course != null).ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> Details(Guid userId)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id != userId.ToString() ||
                !User.IsInRole(Roles.Admin) ||
                !User.IsInRole(Roles.University))
            {
                return Forbid();
            }

            var user = await _context.Users
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == userId.ToString());

            if (user == default)
            {
                return NotFound();
            }

            var reviews = await _context.StudentReviews.Where(x => x.Student.Id == userId.ToString()).ToListAsync();

            var model = new UserDetailsViewModel
            {
                User = user,
                Reviews = reviews
            };

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Edit(Guid userId)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id != userId.ToString() ||
                !User.IsInRole(Roles.Admin) ||
                !User.IsInRole(Roles.University))
            {
                return Forbid();
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == default)
            {
                return BadRequest();
            }

            var model = new EditUserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Patronymic = user.Patronymic,
                Contacts = user.Contacts,
                ShowInApplicantsList = user.ShowInApplicantsList,
                ShowInInternsList = user.ShowInInternsList,
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid userId, EditUserViewModel model)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id != userId.ToString() ||
                !User.IsInRole(Roles.Admin) ||
                !User.IsInRole(Roles.University))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == default)
                {
                    return BadRequest();
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Patronymic = model.Patronymic;
                user.Contacts = model.Contacts;
                user.ShowInApplicantsList = model.ShowInApplicantsList;
                user.ShowInInternsList = model.ShowInInternsList;

                await _context.SaveChangesAsync();

                return View(model);
            }

            return View(model);
        }

        [Authorize(Roles = "Admin, Unversity")]
        public async Task<IActionResult> EditInternshipInfo(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == default)
            {
                return BadRequest();
            }

            var model = new EditInternshipInfoViewModel
            {
                CompanyId = user.CompanyId,
                WorkDirectionId = user.WorkDirectionId
            };

            ViewBag["Companies"] = await _context.Companies.ToListAsync();
            ViewBag["WorkDirections"] = await _context.WorkDirections.ToListAsync();

            return View(model);
        }

        [Authorize(Roles = "Admin, Unversity")]
        [HttpPost]
        public async Task<IActionResult> EditInternshipInfo(Guid userId, EditInternshipInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == default)
                {
                    return BadRequest();
                }

                user.CompanyId = model.CompanyId;
                user.WorkDirectionId = model.WorkDirectionId;

                await _context.SaveChangesAsync();

                return View(model);
            }

            return View(model);
        }
    }
}
