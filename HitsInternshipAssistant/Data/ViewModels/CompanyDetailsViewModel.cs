using HitsInternshipAssistant.Data.Models;

namespace HitsInternshipAssistant.Data.ViewModels
{
    public class CompanyDetailsViewModel
    {
        public Company Company { get; set; }
        public List<ApplicationUser> Interns { get; set; }
    }
}
