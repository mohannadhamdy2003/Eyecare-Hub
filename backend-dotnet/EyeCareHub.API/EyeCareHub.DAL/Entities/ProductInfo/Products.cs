using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Entities.ProductInfo
{
    public class Products : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }

        public int Sales { get; set; } = 0;
        public int MaxQuantity { get; set; }
        public bool TryAR { get; set; }

        public string SideEffect { get; set; }
        public string Disease { get; set; }

        public ProductBrands ProductBrand { get; set; }
        public int ProductBrandId { get; set; }

        public ProductTypes ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public int ProductCategoryId { get; set; }
    }
}
