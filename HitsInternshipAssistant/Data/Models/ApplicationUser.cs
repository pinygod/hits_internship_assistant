namespace HitsInternshipAssistant.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid? CompanyId { get; set; }
        public Company? Company { get; set; }
        public virtual ICollection<Vacancy> VacanciesApplied { get; set; }
    }
}
