using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Entities.OrderAggregate
{
    public class DeliveryMethod : BaseEntity
    {
        public DeliveryMethod()
        {

        }
        public DeliveryMethod(string shortName, string description, string deliveryTime, decimal cost)
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
