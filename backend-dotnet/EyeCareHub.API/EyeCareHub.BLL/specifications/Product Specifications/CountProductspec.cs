using EyeCareHub.DAL.Entities.ProductInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.specifications.Product_Specifications
{
    public class CountProductspec : BaseSpecification<Products>
    {
        public CountProductspec(ProductSpecParams productparams) : base(P =>
        (string.IsNullOrEmpty(productparams.Type) || P.ProductType.Name == productparams.Type) &&
        (string.IsNullOrEmpty((productparams.Brand)) || P.ProductBrand.Name == productparams.Brand) &&
        (string.IsNullOrEmpty((productparams.Category)) || P.ProductCategory.Name == productparams.Category)
        )
        {

        }
    }
}
