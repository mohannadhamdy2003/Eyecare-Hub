using System.ComponentModel.DataAnnotations;

namespace EyeCareHub.API.Dtos.ContentEducations
{
    public class EducationalCategoryAddDto
    {
        
        [Required(ErrorMessage = "Name of category is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description of category is required")]
        public string Description { get; set; }
    }
}
