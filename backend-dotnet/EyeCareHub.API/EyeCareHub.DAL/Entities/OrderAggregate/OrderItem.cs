using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Entities.OrderAggregate
{
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {

        }
        public OrderItem(ProductItemOrder itemOrder, decimal price, int quantity)
        {
            ItemOrder = itemOrder;
            Price = price;
            Quantity = quantity;
        }

        public ProductItemOrder ItemOrder { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

    }
}
