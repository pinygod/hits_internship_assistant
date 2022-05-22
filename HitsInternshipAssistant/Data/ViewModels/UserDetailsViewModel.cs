using HitsInternshipAssistant.Data.Models;

namespace HitsInternshipAssistant.Data.ViewModels
{
    public class UserDetailsViewModel
    {
        public ApplicationUser User { get; set; }
        public ICollection<StudentReview> Reviews { get; set; }
    }
}
