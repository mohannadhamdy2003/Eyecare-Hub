using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.specifications.ArticlesSpecifications
{
    public class SavedArticleParams
    {
        private const int MaxPageSize = 50;
        private int pageSize = 5; // قيمة افتراضية لـ PageSize

        public int PageIndex { get; set; } = 1; // قيمة افتراضية لـ PageIndex

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : (value > 0 ? value : 5); }
        }
        private string search;

        public string Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }

        public int? CategoryId { get; set; }
    }
}
