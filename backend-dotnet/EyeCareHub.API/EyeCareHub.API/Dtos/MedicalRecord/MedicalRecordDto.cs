using EyeCareHub.DAL.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace EyeCareHub.API.Dtos.MedicalRecord
{
    public class MedicalRecordDto
    {

        [Required(ErrorMessage ="Patient Is Required")]
        public int PatientId { get; set; }

        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Diagnosis Is Required")]
        [StringLength(1000)]
        public string Diagnosis { get; set; }

        [StringLength(2000)]
        public string Notes { get; set; }
    }
}
