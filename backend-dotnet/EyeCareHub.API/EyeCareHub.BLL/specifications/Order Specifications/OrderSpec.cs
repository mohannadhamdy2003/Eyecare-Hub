using EyeCareHub.DAL.Entities.Identity;
using EyeCareHub.DAL.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.specifications.Order_Specifications
{
    public class OrderSpec : BaseSpecification<Order>
    {
        public OrderSpec(OrderSpecParams orderSpecParams): base(O =>
        (string.IsNullOrEmpty(orderSpecParams.Search) || O.BuyerEmail.ToLower().Contains(orderSpecParams.Search)) &&
        (string.IsNullOrEmpty(orderSpecParams.Country) || O.ShipToAddress.Country == orderSpecParams.Country) &&
        (string.IsNullOrEmpty(orderSpecParams.BuyerEmail) || O.BuyerEmail == orderSpecParams.BuyerEmail) &&
        (string.IsNullOrEmpty(orderSpecParams.City) || O.ShipToAddress.City == orderSpecParams.City) &&
        (orderSpecParams.Status == null || orderSpecParams.Status.Value.HasFlag(O.Status))
        )
        {
            AddIncludes(o => o.Items);
            AddIncludes(o => o.DeliveryMethod);

            AddOrderBy(P => P.OrderDate);


            if (!string.IsNullOrEmpty(orderSpecParams.Sort))
            {
                switch (orderSpecParams.Sort)
                {
                    case "ByDesc":
                        AddOrderByDesc(P => P.OrderDate);
                        break;
                    default:
                        AddOrderBy(P => P.OrderDate);
                        break;
                }
            }

        }
    }
}
