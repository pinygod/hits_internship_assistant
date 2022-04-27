namespace HitsInternshipAssistant.Data.Models
{
    public class ApplicationUserVacancy
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Guid VacancyId { get; set; }
        public Vacancy Vacancy { get; set; }
    }
}
