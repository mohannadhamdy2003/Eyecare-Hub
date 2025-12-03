using EyeCareHub.DAL.Entities.Identity;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EyeCareHub.API.Dtos.AuthDtos
{
    public class DoctorInfoDto
    {
        [Required(ErrorMessage ="Name Is Required")]
        public string DisplyName { get; set; }
        [Required(ErrorMessage = "specialty Is Required")]
        public string specialty { get; set; }
        [Required(ErrorMessage = "bio Is Required")]
        public string bio { get; set; }

        [Required(ErrorMessage = "Email Is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        public string Password { get; set; }
        public IFormFile Picture { get; set; }

        //[Required(ErrorMessage = "ClinicAddress Is Required")]
        public string ClinicAddress { get; set; }
        
        [Required(ErrorMessage = "YearsOfExperience Is Required")]
        public int YearsOfExperience { get; set; }
        
        [Required(ErrorMessage = "ConsultationFeeIs Required")]
        public decimal ConsultationFee { get; set; }
        
}
}
