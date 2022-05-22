namespace HitsInternshipAssistant.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Contacts { get; set; }
        public bool ShowInApplicantsList { get; set; }
        public bool ShowInInternsList { get; set; }
        public Guid? CompanyId { get; set; }
        public virtual Company? Company { get; set; }
        public Guid? PracticeJournalId { get; set; }
        public virtual PracticeJournal? PracticeJournal { get; set; }
        public Guid? CVId { get; set; }
        public virtual CV? CV { get; set; }
        public virtual ICollection<VacancyApply> AppliedVacancies { get; set; }
    }
}
