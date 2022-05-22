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
        public int? Course { get; set; } = null;
        public Guid? CompanyId { get; set; } = null;
        public virtual Company? Company { get; set; }
        public Guid? WorkDirectionId { get; set; } = null;
        public virtual WorkDirection? WorkDirection { get; set; }
        public Guid? PracticeJournalId { get; set; } = null;
        public virtual PracticeJournal? PracticeJournal { get; set; }
        public Guid? CVId { get; set; } = null;
        public virtual CV? CV { get; set; }
        public virtual ICollection<VacancyApply> AppliedVacancies { get; set; }
    }
}
