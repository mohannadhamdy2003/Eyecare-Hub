using EyeCareHub.DAL.Entities.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EyeCareHub.API.Dtos.AuthDtos
{
    public class PatientRegistrationDto
    {
        [Required(ErrorMessage ="Age Is Required")]
        public int Age { get; set; }

        public MedicalHistory MedicalHistory { get; set; }

        public string OtherMedicalHistory { get; set; } 
    }
}
