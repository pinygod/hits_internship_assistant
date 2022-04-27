using HitsInternshipAssistant.Data.Models;

namespace HitsInternshipAssistant.Data.ViewModels
{
    public class EditCVViewModel
    {
        public string AdditionalInfo { get; set; }
        public string Contacts { get; set; }
        public ICollection<StudentWorkDirection> WorkDirections { get; set; }
    }
}
