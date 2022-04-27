#nullable disable
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
using Microsoft.AspNetCore.Identity;
using HitsInternshipAssistant.Data.ViewModels;

namespace HitsInternshipAssistant.Controllers
{
    public class CVsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CVsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin, University, HR")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.CVs.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> Details(Guid? id)
        {
            CV cv = await _context.CVs.FirstOrDefaultAsync(x => x.Id == id);
            if (cv == default)
            {
                return NotFound();
            }

            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user.Id != cv.UserId.ToString() &&
                !User.IsInRole(Roles.Admin) &&
                !User.IsInRole(Roles.University) &&
                !User.IsInRole(Roles.HR))
            {
                return Forbid();
            }

            return View(cv);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCVViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);

                CV cv = new()
                {
                    AdditionalInfo = model.AdditionalInfo,
                    Contacts = model.Contacts
                };

                foreach (var direction in model.WorkDirections)
                {
                    direction.CVId = cv.Id;
                    _context.StudentWorkDirections.Add(direction);
                    cv.WorkDirections.Add(direction);
                }

                _context.Add(cv);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            CV cv = await _context.CVs.FirstOrDefaultAsync(x => x.Id == id);
            if (cv == default)
            {
                return NotFound();
            }

            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user.Id != cv.UserId.ToString() && !User.IsInRole(Roles.Admin))
            {
                return Forbid();
            }

            EditCVViewModel model = new()
            {
                AdditionalInfo = cv.AdditionalInfo,
                Contacts = cv.Contacts,
                WorkDirections = cv.WorkDirections
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditCVViewModel model)
        {
            if (ModelState.IsValid)
            {
                CV cv = await _context.CVs.FirstOrDefaultAsync(x => x.Id == id);
                if (cv == default)
                {
                    return NotFound();
                }

                ApplicationUser user = await _userManager.GetUserAsync(User);
                if (user.Id != cv.UserId.ToString() && !User.IsInRole(Roles.Admin))
                {
                    return Forbid();
                }

                cv.AdditionalInfo = model.AdditionalInfo;
                cv.Contacts = model.Contacts;

                var newDirections = model.WorkDirections.Where(x => cv.WorkDirections.Any(z => z.Id == x.Id));
                var directionsToRemove = cv.WorkDirections.Where(x => model.WorkDirections.Any(z => z.Id == x.Id));

                _context.StudentWorkDirections.RemoveRange(directionsToRemove);
                foreach (var direction in newDirections)
                {
                    direction.CVId = cv.Id;
                    _context.StudentWorkDirections.Add(direction);
                    cv.WorkDirections.Add(direction);
                }

                cv.WorkDirections = model.WorkDirections;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Delete(Guid? id)
        {
            CV cv = await _context.CVs.FirstOrDefaultAsync(x => x.Id == id);
            if (cv == default)
            {
                return NotFound();
            }

            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user.Id != cv.UserId.ToString() && !User.IsInRole(Roles.Admin))
            {
                return Forbid();
            }

            return View(cv);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            CV cv = await _context.CVs.FirstOrDefaultAsync(x => x.Id == id);
            if (cv == default)
            {
                return NotFound();
            }

            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user.Id != cv.UserId.ToString() && !User.IsInRole(Roles.Admin))
            {
                return Forbid();
            }

            _context.CVs.Remove(cv);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
