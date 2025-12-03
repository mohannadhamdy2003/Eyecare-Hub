using EyeCareHub.DAL.Entities.Content_Education;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.specifications.ArticlesSpecifications
{
    public class CountArticleSpec : BaseSpecification<Article>
    {
        public CountArticleSpec(ArticleSpecParams articleparams) : base(P =>
        (string.IsNullOrEmpty(articleparams.Search) || P.Title.ToLower().Contains(articleparams.Search)) &&
        (!articleparams.CategoryId.HasValue || P.CategoryId == articleparams.CategoryId.Value))
        {
            
        }
    }
}
