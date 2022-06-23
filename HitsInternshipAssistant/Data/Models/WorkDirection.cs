namespace HitsInternshipAssistant.Data.Models
{
    public class WorkDirection
    {
        public Guid Id { get; set; } = new Guid();
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<StudentWorkDirection> StudentWorkDirections { get; set; }
    }
}
