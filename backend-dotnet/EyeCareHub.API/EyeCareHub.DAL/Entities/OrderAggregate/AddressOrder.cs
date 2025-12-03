using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Entities.OrderAggregate
{
    public class AddressOrder
    {
        public AddressOrder()
        {

        }
        public AddressOrder(string country, string city, string street)
        {
            Country = country;
            City = city;
            Street = street;
        }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }
}
