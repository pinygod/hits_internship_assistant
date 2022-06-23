namespace HitsInternshipAssistant.Data.Models
{
    public class CompanySpeech
    {
        public Guid Id { get; set; } = new Guid();
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
