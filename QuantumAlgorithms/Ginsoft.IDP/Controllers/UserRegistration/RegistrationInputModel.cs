namespace Ginsoft.IDP.Controllers.UserRegistration
{
    public class RegistrationInputModel
    {
        public string ReturnUrl { get; set; }
        public string Provider { get; set; }
        public string ProviderUserId { get; set; }
        public string Email { get; set; }
        public bool IsExternalProvider => !string.IsNullOrEmpty(Provider);
    }
}
