using EyeCareHub.DAL.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.specifications.Order_Specifications
{
    public class OrderCountSpec : BaseSpecification<Order>
    {
        public OrderCountSpec(OrderSpecParams orderSpecParams) : base(O =>
        (string.IsNullOrEmpty(orderSpecParams.Search) || O.BuyerEmail.ToLower().Contains(orderSpecParams.Search)) &&
        (string.IsNullOrEmpty(orderSpecParams.Country) || O.ShipToAddress.Country == orderSpecParams.Country) &&
        (string.IsNullOrEmpty(orderSpecParams.BuyerEmail) || O.BuyerEmail == orderSpecParams.BuyerEmail) &&
        (string.IsNullOrEmpty(orderSpecParams.City) || O.ShipToAddress.City == orderSpecParams.City) &&
        (orderSpecParams.Status == null || orderSpecParams.Status.Value.HasFlag(O.Status))
        )
        {
            
        }

    }
}
