using EyeCareHub.DAL.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.Interface
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> AddItemsToBasketAsync(string userId, BasketItem newItems);
        Task<CustomerBasket> RemoveItemFromBasketAsync(string userId, int itemId);
        Task<CustomerBasket> UpdateItemQuantityAsync(string userId, int itemId, int newQuantity);


        Task<CustomerBasket> GetCustomerBasket(string BasketId);
        Task<CustomerBasket> UpdateCustomerBasket(CustomerBasket Basket);
        Task<bool> DeleteCustomerBasket(string BasketId);
    }
}
