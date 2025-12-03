using System;

namespace EyeCareHub.API.Dtos.OrderDtos
{
    public class DeliveryMethodDto
    {
        public int Id { get; set; }
        public DeliveryMethodDto()
        {

        }
        public DeliveryMethodDto(string shortName, string description, string deliveryTime, decimal cost)
        {
            ShortName = shortName;
            Description = description;
            DeliveryTime = deliveryTime;
            Cost = cost;
        }

        public string ShortName { get; set; }
        public string Description { get; set; }
        public string DeliveryTime { get; set; }
        public Decimal Cost { get; set; }
    }
}
