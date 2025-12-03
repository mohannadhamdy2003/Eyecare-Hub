using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.specifications.Product_Specifications
{
    public class ProductSpecParams
    {
        private const int MaxPageSize = 50;
        private int pageSize = 5; // قيمة افتراضية لـ PageSize

        public int PageIndex { get; set; } = 1; // قيمة افتراضية لـ PageIndex

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : (value > 0 ? value : 5); }
        }

        public string Sort { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
    }
}
