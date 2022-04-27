namespace HitsInternshipAssistant.Data.Models
{
    public class StudentWorkDirection
    {
        public Guid Id { get; set; } = new Guid();
        public Guid ParentDirectionId { get; set; }
        public WorkDirection ParentDirection { get; set; }
        public Guid CVId { get; set; }
        public CV CV { get; set; }
        public string Experience { get; set; }
        public string Stack { get; set; }
    }
}
