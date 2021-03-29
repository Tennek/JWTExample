using System.ComponentModel.DataAnnotations;

namespace JWTSandbox.Authentication.API.Services.Models
{
    public class Audience
    {
        [Key]
        [MaxLength(32)]
        public string AudienceId { get; set; }

        [MaxLength(80)]
        [Required]
        public string AudienceSecret { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
    }
}
