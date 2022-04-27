namespace HitsInternshipAssistant.Data.Models
{
    public class Company
    {
        public Guid Id { get; set; } = new Guid();
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public Guid HRId { get; set; }
        public ApplicationUser HR { get; set; }
        public virtual ICollection<Vacancy> Vacancies { get; set; }
    }
}
