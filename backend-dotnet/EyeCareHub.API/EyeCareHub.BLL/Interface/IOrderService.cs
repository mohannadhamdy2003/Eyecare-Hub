using EyeCareHub.DAL.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.Interface
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethods, AddressOrder shipToAddress);
        Task<IReadOnlyList<Order>> GetOrderForUserAsync(string BuyerEmail);
        Task<Order> GetActiveOrderAsync(string emailBuyer);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
        Task<bool> AnyGetDeliveryMethodsForIdAsync(int Id);
    }
}
