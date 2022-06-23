namespace HitsInternshipAssistant.Data.ViewModels
{
    public class CreateCompanyViewModel
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Tagline { get; set; }
        public IFormFile BackgroundLogo { get; set; }
        public IFormFile Logo { get; set; }
        public string Description { get; set; }
        public string Contacts { get; set; }
        public int PartnershipStartYear { get; set; }
    }
}
