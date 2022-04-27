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
            if (user.Id != vacancyApply.User.Id.ToString() &&
                !User.IsInRole(Roles.Admin) &&
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
            if (user.Id != vacancyApply.User.Id.ToString() &&
                !User.IsInRole(Roles.Admin) &&
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
