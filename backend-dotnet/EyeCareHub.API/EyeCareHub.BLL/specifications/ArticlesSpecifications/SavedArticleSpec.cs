using EyeCareHub.DAL.Entities.Content_Education;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.specifications.ArticlesSpecifications
{
    public  class SavedArticleSpec : BaseSpecification<Article>
    {
        public SavedArticleSpec(SavedArticleParams savedArticleParams , string currentUserEmail) : base(P =>
        (string.IsNullOrEmpty(savedArticleParams.Search) || P.Title.ToLower().Contains(savedArticleParams.Search)) &&
        (!savedArticleParams.CategoryId.HasValue || P.CategoryId == savedArticleParams.CategoryId.Value) &&
        (P.SavedArticles.Any(x=> x.UserEmail == currentUserEmail)))
        {

        }

    }
}
