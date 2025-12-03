using System;
using System.ComponentModel.DataAnnotations;

namespace EyeCareHub.API.Dtos.OrderDtos
{
    public class DeliveryMethodToAddDto
    {
        [Required(ErrorMessage = "ShortName Is Required")]
        public string ShortName { get; set; }
        [Required(ErrorMessage = "Description Is Required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "DeliveryTime Is Required")]
        public string DeliveryTime { get; set; }
        [Required(ErrorMessage = "Cost Is Required")]
        public Decimal Cost { get; set; }
    }
}
