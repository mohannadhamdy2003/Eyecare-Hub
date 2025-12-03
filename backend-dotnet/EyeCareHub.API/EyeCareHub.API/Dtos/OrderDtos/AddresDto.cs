using System.ComponentModel.DataAnnotations;

namespace EyeCareHub.API.Dtos.OrderDtos
{
    public class AddresDto
    {
        public AddresDto()
        {

        }
        public AddresDto(string country, string city, string street)
        {
            Country = country;
            City = city;
            Street = street;
        }
        [Required(ErrorMessage = "Country Is Requery")]
        public string Country { get; set; }

        [Required(ErrorMessage = "City Is Requery")]
        public string City { get; set; }

        [Required(ErrorMessage = "Street Is Requery")]
        public string Street { get; set; }
    }
}
