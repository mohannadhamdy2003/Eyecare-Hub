namespace EyeCareHub.API.Dtos.OrderDtos
{
    public class UpdateOrderStatusDto
    {
        public int OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
