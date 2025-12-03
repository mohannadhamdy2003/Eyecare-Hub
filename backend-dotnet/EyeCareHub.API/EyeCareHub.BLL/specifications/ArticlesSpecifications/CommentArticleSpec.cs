using EyeCareHub.DAL.Entities.Content_Education;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.specifications.ArticlesSpecifications
{
    public class CommentArticleSpec :BaseSpecification<CommentsArticle>
    {
        public CommentArticleSpec(CommentArticleParams commentparams) :base(P => 
        (commentparams.articleId!=0 || P.ArticleId == commentparams.articleId))

        {
            ApplyPagination((commentparams.PageSize * (commentparams.PageIndex - 1)), commentparams.PageSize);
            AddIncludes(A => A.User);
        }
    }
}
