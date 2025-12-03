using System.ComponentModel.DataAnnotations;

namespace EyeCareHub.API.Dtos.Product
{
    public class TypeDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name of Type Is Required")]
        public string Name { get; set; }
    }
}
