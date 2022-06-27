using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HitsInternshipAssistant.Data;
using HitsInternshipAssistant.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using HitsInternshipAssistant.Services;

namespace HitsInternshipAssistant.Controllers
{
    public class PracticeJournalsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly FileUploadsService _fileUploadsService;

        public PracticeJournalsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, FileUploadsService fileUploadsService)
        {
            _context = context;
            _userManager = userManager;
            _fileUploadsService = fileUploadsService;
        }

        [Authorize(Roles = "Admin, University")]
        public async Task<IActionResult> Index()
        {
            return View(
                await _context.PracticeJournal.Include(x => x.Student)
                .ThenInclude(c => c.Company)
                .Where(p => p.Status != PracticeJournalStatus.Empty)
                .ToListAsync()
                );
        }

        [Authorize]
        public async Task<IActionResult> Details(Guid studentId)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            ApplicationUser userTwo = await _userManager.FindByIdAsync(studentId.ToString());
            if (userTwo == null)
            {
                return BadRequest();
            }

            if (user?.Id != studentId.ToString() &&
                !User.IsInRole(Roles.Admin) &&
                !User.IsInRole(Roles.University))
            {
                return Forbid();
            }

            var practiceJournal = await _context.PracticeJournal
                .Include(x => x.Student)
                .FirstOrDefaultAsync(m => m.Student.Id == studentId.ToString());

            if (practiceJournal == default)
            {
                ApplicationUser student = await _userManager.FindByIdAsync(studentId.ToString());

                practiceJournal = new()
                {
                    Student = student,
                    Status = PracticeJournalStatus.Empty

                };

                _context.Add(practiceJournal);
                await _context.SaveChangesAsync();
            }

            return View(practiceJournal);
        }

        [Authorize(Roles = "Admin, University")]
        [HttpPost]
        public async Task<IActionResult> AddReview(Guid id, string review)
        {
            var practiceJournal = await _context.PracticeJournal.Include(s => s.Student).FirstOrDefaultAsync(p => p.Id == id);
            if (practiceJournal != default)
            {
                practiceJournal.Review = review;

                await _context.SaveChangesAsync();

                return RedirectToAction("Details", new { studentId = practiceJournal.Student.Id });
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin, University")]
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(Guid id, PracticeJournalStatus status)
        {
            var practiceJournal = await _context.PracticeJournal.Include(s => s.Student).FirstOrDefaultAsync(p => p.Id == id);
            if (practiceJournal != default)
            {
                practiceJournal.Status = status;

                await _context.SaveChangesAsync();

                return RedirectToAction("Details", new { studentId = practiceJournal.Student.Id });
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddFile(Guid id, IFormFile file)
        {
            var practiceJournal = await _context.PracticeJournal.FindAsync(id);
            if (practiceJournal != default)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                if (user.Id != practiceJournal.Student.Id &&
                    !User.IsInRole(Roles.Admin) &&
                    !User.IsInRole(Roles.University))
                {
                    return Forbid();
                }

                practiceJournal.FileLink = await _fileUploadsService.UploadImageAsync(file);

                await _context.SaveChangesAsync();

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
