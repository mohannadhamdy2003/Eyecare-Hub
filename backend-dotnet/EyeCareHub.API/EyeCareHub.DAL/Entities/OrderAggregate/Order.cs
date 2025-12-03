using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Entities.OrderAggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {

        }
        public Order(string buyerEmail, AddressOrder shipToAddress, DeliveryMethod deliveryMethod, List<OrderItem> items, decimal subtotal)
        {
            BuyerEmail = buyerEmail;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            Subtotal = subtotal;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        public AddressOrder ShipToAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public List<OrderItem> Items { get; set; }

        public int PaymentIntentId { get; set; }
        public decimal Subtotal { get; set; }

        public decimal GetTotal()
            => Subtotal + DeliveryMethod.Cost;

    }
}
