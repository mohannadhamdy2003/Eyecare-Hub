using EyeCareHub.BLL.specifications.Product_Specifications;
using EyeCareHub.DAL.Entities.Content_Education;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.specifications.ArticlesSpecifications
{
    public class ArticlesWithCategorySpec :BaseSpecification<Article>
    {
        public ArticlesWithCategorySpec(ArticleSpecParams articleparams) : base(P =>
        (string.IsNullOrEmpty(articleparams.Search) || P.Title.ToLower().Contains(articleparams.Search)) &&
        (!articleparams.CategoryId.HasValue || P.CategoryId == articleparams.CategoryId.Value))
        {
            AddIncludes(P => P.Category);

            ApplyPagination((articleparams.PageSize * (articleparams.PageIndex - 1)), articleparams.PageSize);

            AddOrderBy(P => P.LovesCount);
         }


    public ArticlesWithCategorySpec(int id) : base(P => P.Id == id)
    {
        AddIncludes(P => P.Category);
    }
}
}
