namespace HitsInternshipAssistant.Data.Models
{
    public class Vacancy
    {
        public Guid Id { get; set; } = new Guid();
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
        public virtual ICollection<ApplicationUser> Applicants { get; set; }
    }
}
