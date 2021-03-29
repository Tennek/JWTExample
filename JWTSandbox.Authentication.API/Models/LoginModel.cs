using System.ComponentModel.DataAnnotations;

namespace JWTSandbox.Authentication.API.Models
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [MaxLength(100)]
        public string AudienceId { get; set; }
    }
}
