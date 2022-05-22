namespace HitsInternshipAssistant.Data.Models
{
    public class PracticeJournal
    {
        public Guid Id { get; set; } = new Guid();
        public ApplicationUser Student { get; set; }
        public string? FileLink { get; set; }
        public string? Review { get; set; }
        public PracticeJournalStatus Status { get; set; }
    }
}
