using EyeCareHub.DAL.Entities.OrderAggregate;
using System.Collections.Generic;
using System;

namespace EyeCareHub.API.Dtos.OrderDtos
{
    public class OrderToReturnDto
    {
        public DateTimeOffset OrderDate { get; set; }

        public AddresDto ShipToAddress { get; set; }
        public DeliveryMethodDto DeliveryMethod { get; set; }

        public OrderStatus Status { get; set; } 

        public List<OrderItemDto> Items { get; set; }

        public int PaymentIntentId { get; set; }
        public decimal Subtotal { get; set; }

        public decimal GetTotal()
            => Subtotal + DeliveryMethod.Cost;
    }
}
