using EyeCareHub.BLL.Interface;
using EyeCareHub.DAL.Data;
using EyeCareHub.DAL.Entities.OrderAggregate;
using EyeCareHub.DAL.Entities.ProductInfo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EyeCareHub.BLL.Repositories
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork<StoreContext> _unitOfWork;

        public OrderService(IBasketRepository basketRepo, IUnitOfWork<StoreContext> unitOfWork)
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
        }



        public async Task<Order> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethods, AddressOrder shipToAddress)
        {
            // Get Basket FromBasketRepo
            var Basket = await _basketRepo.GetCustomerBasket(BasketId);
            // Get Selected Items at Basket From Product Repo
            var orderItems = new List<OrderItem>();

            foreach (var Item in Basket.Items)
            {
                var Products = await _unitOfWork.Repository<Products>().GetByIdAsync(Item.ProductId);
                var ProductItems = new ProductItemOrder(Products.Id, Products.Name, Products.PictureUrl);
                var orderItem = new OrderItem(ProductItems, Products.Price, Item.Quantity);
                orderItems.Add(orderItem);
            }
            // Delivery Methods From DeliveryMethod Repo

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethods);

            // Calculate SubTota


            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);
            // Create Order
            var Order = new Order(BuyerEmail, shipToAddress, deliveryMethod, orderItems, subTotal);

            await _unitOfWork.Repository<Order>().Add(Order);
            // Save To DataBase

            await _unitOfWork.Complete();
            return Order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
        }

        public async Task<bool> AnyGetDeliveryMethodsForIdAsync(int Id)
        {
            return await _unitOfWork.Repository<DeliveryMethod>().AnyAsync(o => o.Id == Id);
            
        }

        public async Task<Order> GetActiveOrderAsync(string emailBuyer)
        {
            var query = _unitOfWork.Repository<Order>().GetQueryable().Include(o => o.DeliveryMethod).Include(o => o.Items)

                .Where(o => o.BuyerEmail == emailBuyer)
                .Where(o => o.Status == OrderStatus.Pending
                 || o.Status == OrderStatus.PaymentReceived
                 || o.Status == OrderStatus.Shipped);

            return await query.FirstOrDefaultAsync();
        }


        public async Task<IReadOnlyList<Order>> GetOrderForUserAsync(string BuyerEmail)
        {
            var query = _unitOfWork.Repository<Order>().GetQueryable().Include(o => o.DeliveryMethod).Include(o => o.Items)

                .Where(o => o.BuyerEmail == BuyerEmail);

            return await query.ToListAsync();
        }
    }

}
