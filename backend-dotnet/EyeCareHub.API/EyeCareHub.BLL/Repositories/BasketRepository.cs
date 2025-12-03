using EyeCareHub.BLL.Interface;
using EyeCareHub.DAL.Entities.Basket;
using EyeCareHub.DAL.Entities.Identity;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.Repositories
{
    public class BasketRepository:IBasketRepository
    {
        private readonly IDatabase Database;
        private readonly string KeyPrefix;

        public BasketRepository(IConnectionMultiplexer Redis, IConfiguration configuration)
        {
            Database = Redis.GetDatabase();

            KeyPrefix = configuration.GetSection("BasketSettings:BasketKeyPrefix").Value;
        }

        /// /////////////////////////// 

        public async Task<CustomerBasket> AddItemsToBasketAsync(string userId, BasketItem newItems)
        {
            var basket = await GetCustomerBasket(userId) ?? new CustomerBasket(userId);

            
            var existingItem = basket.Items.FirstOrDefault(x => x.ProductId == newItems.ProductId);
            if (existingItem != null)
            {
                // لو العنصر موجود، زود الكمية
                existingItem.Quantity ++;
            }
            else
            {
                // لو مش موجود، أضفه
                basket.Items.Add(newItems);
            }
            

            return await UpdateCustomerBasket(basket);
        }

        public async Task<CustomerBasket> RemoveItemFromBasketAsync(string userId, int itemId)
        {
            var basket = await GetCustomerBasket(userId);
            if (basket == null) return null;

            var item = basket.Items.FirstOrDefault(x => x.ProductId == itemId);
            if (item != null)
            {
                basket.Items.Remove(item);
                return await UpdateCustomerBasket(basket);
            }

            return basket;
        }

        public async Task<CustomerBasket> UpdateItemQuantityAsync(string userId, int itemId, int newQuantity)
        {
            var basket = await GetCustomerBasket(userId);
            if (basket == null) return null;

            var item = basket.Items.FirstOrDefault(x => x.ProductId == itemId);
            if (item != null)
            {
                item.Quantity = newQuantity;
                return await UpdateCustomerBasket(basket);
            }

            return basket;
        }



        ///////////////////////////
        
        public async Task<bool> DeleteCustomerBasket(string UserId)
        {
            return await Database.KeyDeleteAsync(UserId);
        }


        public async Task<CustomerBasket> GetCustomerBasket(string UserId)
        {
            
            var basket = await Database.StringGetAsync(UserId);
            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket> UpdateCustomerBasket(CustomerBasket Basket)
        {
            var customBasket = await Database.StringSetAsync(Basket.Id, JsonSerializer.Serialize(Basket), TimeSpan.FromDays(30));
            if (!customBasket) return null;
            return await GetCustomerBasket(Basket.Id);
        }
        
    }
}
