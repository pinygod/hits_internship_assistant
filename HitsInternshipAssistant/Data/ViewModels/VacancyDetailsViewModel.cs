using HitsInternshipAssistant.Data.Models;

namespace HitsInternshipAssistant.Data.ViewModels
{
    public class VacancyDetailsViewModel
    {
        public Vacancy Vacancy { get; set; }
        public VacancyApply? VacancyApply { get; set; } = null;
    }
}
