using EyeCareHub.DAL.Entities.Content_Education;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.specifications.ArticlesSpecifications
{
    public class CommentArticleCountSpec :BaseSpecification<CommentsArticle>
    {
        public CommentArticleCountSpec(CommentArticleParams commentparams) : base(P =>
        (commentparams.articleId != 0 || P.ArticleId == commentparams.articleId))
        {

        }
    }
}
