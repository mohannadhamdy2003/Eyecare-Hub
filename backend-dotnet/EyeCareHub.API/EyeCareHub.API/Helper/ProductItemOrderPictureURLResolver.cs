using AutoMapper;
using EyeCareHub.API.Dtos.OrderDtos;
using EyeCareHub.API.Dtos.Product;
using EyeCareHub.DAL.Entities.OrderAggregate;
using EyeCareHub.DAL.Entities.ProductInfo;
using Microsoft.Extensions.Configuration;

namespace EyeCareHub.API.Helper
{
    public class ProductItemOrderPictureURLResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration configration;

        public ProductItemOrderPictureURLResolver(IConfiguration configration)
        {
            this.configration = configration;
        }

        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ItemOrder.PictureUrl))
                return $"{configration["BaseURL"]}{source.ItemOrder.PictureUrl}";
            return null;
        }
    }

}
