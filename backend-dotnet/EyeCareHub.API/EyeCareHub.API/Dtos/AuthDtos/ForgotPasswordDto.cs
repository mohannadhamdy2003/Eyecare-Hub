using System.ComponentModel.DataAnnotations;

namespace EyeCareHub.API.Dtos.AuthDtos
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage ="Email Is Required")]
        public string Email { get; set; }
    }
}
