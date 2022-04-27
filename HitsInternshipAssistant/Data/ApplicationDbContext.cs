using HitsInternshipAssistant.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HitsInternshipAssistant.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }
        public DbSet<WorkDirection> WorkDirections { get; set; }
        public DbSet<StudentWorkDirection> StudentWorkDirections { get; set; }
        public DbSet<CV> CVs { get; set; }
        public DbSet<VacancyApply> VacancyApplies { get; set; }
    }
}