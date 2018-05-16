using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ginsoft.IDP.Controllers.UserRegistration
{
    public class RegisterUserViewModel
    {
        // credentials       
        [MaxLength(100)]
        public string Username { get; set; }

        [MaxLength(100)]
        public string Password { get; set; }

        // claims 
        [Required]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nickname { get; set; }

        public string ReturnUrl { get; set; }

        public string Provider { get; set; }
        public string ProviderUserId { get; set; }
        public bool IsExternalProvider { get { return !string.IsNullOrEmpty(Provider); } }

    }
}
