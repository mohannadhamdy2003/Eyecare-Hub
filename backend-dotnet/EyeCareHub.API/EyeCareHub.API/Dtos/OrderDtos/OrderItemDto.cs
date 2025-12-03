using EyeCareHub.DAL.Entities.OrderAggregate;

namespace EyeCareHub.API.Dtos.OrderDtos
{
    public class OrderItemDto
    {
        public OrderItemDto()
        {

        }
        public OrderItemDto(ProductItemOrderDto itemOrder, decimal price, int quantity)
        {
            ItemOrder = itemOrder;
            Price = price;
            Quantity = quantity;
        }

        public ProductItemOrderDto ItemOrder { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
