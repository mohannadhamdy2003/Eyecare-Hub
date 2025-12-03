using System.ComponentModel.DataAnnotations;

namespace EyeCareHub.API.Dtos
{
    public class RestPasswordDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string token { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
