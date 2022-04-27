using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HitsInternshipAssistant.Data.Models
{
    public class VacancyApply
    {
        public Guid Id { get; set; } = new Guid();
        public virtual ApplicationUser User { get; set; }
        public Guid VacancyId { get; set; }
        public virtual Vacancy Vacancy { get; set; }
        public VacancyApplyStatus Status { get; set; }
    }
}
