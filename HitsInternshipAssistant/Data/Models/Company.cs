namespace HitsInternshipAssistant.Data.Models
{
    public class Company
    {
        public Guid Id { get; set; } = new Guid();
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Tagline { get; set; }
        public string BackgroundLogoLink { get; set; }
        public string LogoLink { get; set; }
        public string Description { get; set; }
        public string Contacts { get; set; }
        public int PartnershipStartYear { get; set; }
        public virtual ICollection<ApplicationUser> Employees { get; set; }
        public virtual ICollection<Vacancy> Vacancies { get; set; }
        public virtual ICollection<CompanySpeech> Speeches { get; set; }
    }
}
