﻿namespace HitsInternshipAssistant.Data.Models
{
    public class CV
    {
        public Guid Id { get; set; } = new Guid();
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string AdditionalInfo { get; set; }
        public string Contacts { get; set; }
        public virtual ICollection<StudentWorkDirection> WorkDirections { get; set; }
    }
}