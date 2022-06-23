namespace HitsInternshipAssistant.Data.Models
{
    public class StudentReview
    {
        public Guid Id { get; set; } = new Guid();
        public ApplicationUser Student { get; set; }
        public ApplicationUser Reviewer { get; set; }
        public string Review { get; set; }
    }
}
