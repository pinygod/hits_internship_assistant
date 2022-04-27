namespace HitsInternshipAssistant.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid? CompanyId { get; set; }
        public virtual Company? Company { get; set; }
        public Guid? CVId { get; set; }
        public virtual CV CV { get; set; }
        public virtual ICollection<VacancyApply> AppliedVacancies { get; set; }
    }
}
