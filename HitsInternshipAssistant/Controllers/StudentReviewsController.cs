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
    public class StudentReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentReviewsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin, University")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.StudentReviews.Include(s => s.Student).ToListAsync());
        }

        [Authorize(Roles = "Admin, University")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.StudentReviews == null)
            {
                return NotFound();
            }

            var studentReview = await _context.StudentReviews
                .Include(x => x.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentReview == null)
            {
                return NotFound();
            }

            return View(studentReview);
        }

        [Authorize(Roles = "Admin, University, HR")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin, University, HR")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateStudentReviewViewModel model)
        {
            if (ModelState.IsValid)
            {
                var student = await _userManager.FindByIdAsync(model.StudentId.ToString());
                var reviewer = await _userManager.GetUserAsync(User);

                StudentReview review = new()
                {
                    Review = model.Review,
                    Student = student,
                    Reviewer = reviewer
                };

                _context.Add(review);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = "Admin, University")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.StudentReviews == null)
            {
                return NotFound();
            }

            var studentReview = await _context.StudentReviews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentReview == null)
            {
                return NotFound();
            }

            return View(studentReview);
        }

        [Authorize(Roles = "Admin, University")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.StudentReviews == null)
            {
                return Problem("Entity set 'ApplicationDbContext.StudentReviews'  is null.");
            }
            var studentReview = await _context.StudentReviews.FindAsync(id);
            if (studentReview != null)
            {
                _context.StudentReviews.Remove(studentReview);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
