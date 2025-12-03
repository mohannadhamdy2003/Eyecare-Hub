using System.ComponentModel.DataAnnotations;

namespace EyeCareHub.API.Dtos.Product
{
    public class BrandDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name of Brand Is Required")]
        public string Name { get; set; }
    }
}
