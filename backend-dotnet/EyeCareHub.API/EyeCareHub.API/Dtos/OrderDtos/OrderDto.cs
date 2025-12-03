namespace EyeCareHub.API.Dtos.OrderDtos
{
    public class OrderDto
    {
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddresDto ShipToAddress { get; set; }
    }
}
