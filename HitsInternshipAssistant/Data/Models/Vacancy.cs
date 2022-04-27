﻿namespace HitsInternshipAssistant.Data.Models
{
    public class Vacancy
    {
        public Guid Id { get; set; } = new Guid();
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<VacancyApply> Applicants { get; set; }
    }
}
