using AutoMapper;
using Microsoft.Extensions.Configuration;
using EyeCareHub.API.Dtos.Product;
using EyeCareHub.DAL.Entities.ProductInfo;

namespace EyeCareHub.API.Helper
{
    public class ProductPictureURLResolver : IValueResolver<Products, ProductToReturn, string>
    {
        private readonly IConfiguration _config;

        public ProductPictureURLResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Products source, ProductToReturn destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_config["BaseURL"]}{source.PictureUrl}";
            }

            return null;
        }
    }
}
