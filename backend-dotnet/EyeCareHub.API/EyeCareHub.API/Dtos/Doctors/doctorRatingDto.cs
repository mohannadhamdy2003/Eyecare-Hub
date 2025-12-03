using System.ComponentModel.DataAnnotations;

namespace EyeCareHub.API.Dtos.Doctors
{
    public class doctorRatingDto
    {
        [Required(ErrorMessage ="DoctorId iS required")]
        public int doctorId { get; set; }
        [Required(ErrorMessage ="Rating Is Required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public decimal ratingValue { get; set; }
        [Required(ErrorMessage = "Comments is required")]
        [StringLength(40, ErrorMessage = "Comments cannot exceed 40 characters")]
        public string Comments { get; set; }
    }
}
