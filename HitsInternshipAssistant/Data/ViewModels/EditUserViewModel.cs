using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HitsInternshipAssistant.Data.ViewModels
{
    public class EditUserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Contacts { get; set; }
        public bool ShowInApplicantsList { get; set; } = false;
        public bool ShowInInternsList { get; set; } = false;
    }
}
