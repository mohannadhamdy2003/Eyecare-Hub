using System.ComponentModel.DataAnnotations;

namespace EyeCareHub.API.Dtos.Product
{
    public class CategoryDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name of Category Is Required")]
        public string Name { get; set; }
    }
}
