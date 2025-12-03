using EyeCareHub.DAL.Entities.ProductInfo;
using Microsoft.AspNetCore.Http;

namespace EyeCareHub.API.Dtos.Product
{
    public class ProductDto
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile PictureUrl { get; set; }

        public int Sales { get; set; } = 0;
        public int MaxQuantity { get; set; }
        public bool TryAR { get; set; }
        public string SideEffect { get; set; }
        public string Disease { get; set; }

        public int ProductBrandId { get; set; }
        public int ProductTypeId { get; set; }
        public int ProductCategoryId { get; set; }

    }
}
