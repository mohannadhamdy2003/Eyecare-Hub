using EyeCareHub.DAL.Entities.ProductInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.specifications.Product_Specifications
{
    public class ProductWithTypeAndBrandSpec: BaseSpecification<Products>
    {
        public ProductWithTypeAndBrandSpec(ProductSpecParams productparams) : base(P =>
        (string.IsNullOrEmpty(productparams.Type) || P.ProductType.Name == productparams.Type) &&
        (string.IsNullOrEmpty((productparams.Brand)) || P.ProductBrand.Name == productparams.Brand) &&
        (string.IsNullOrEmpty((productparams.Category)) || P.ProductCategory.Name == productparams.Category))
        {
            AddIncludes(P => P.ProductType);
            AddIncludes(P => P.ProductBrand);
            AddIncludes(P => P.ProductCategory);

            AddOrderBy(P => P.Sales);

            ApplyPagination((productparams.PageSize * (productparams.PageIndex - 1)), productparams.PageSize);

            if (!string.IsNullOrEmpty(productparams.Sort))
            {
                switch (productparams.Sort)
                {
                    case "PriceBy":
                        AddOrderBy(P => P.Price);
                        break;
                    case "PriceByDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Sales);
                        break;
                }
            }
        }


        public ProductWithTypeAndBrandSpec(int id) : base(P => P.Id == id)
        {
            AddIncludes(P => P.ProductType);
            AddIncludes(P => P.ProductBrand);
        }
    }
}
