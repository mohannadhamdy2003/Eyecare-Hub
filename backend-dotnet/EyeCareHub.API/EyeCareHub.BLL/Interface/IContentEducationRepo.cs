using EyeCareHub.BLL.specifications.ArticlesSpecifications;
using EyeCareHub.DAL.Entities.Content_Education;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.Interface
{
    public interface IContentEducationRepo
    {
        #region EducationalCategory
        Task<IReadOnlyList<EducationalCategory>> GeAllEducationalCategory();
        Task<EducationalCategory> GetByIdEducationalCategory(int Id);
        Task<bool> AddEducationalCategory(EducationalCategory educationalCategory);
        Task<bool> UpdateEducationalCategory(EducationalCategory educationalCategory);
        Task<bool> DeleteEducationalCategory(int educationalCategoryId);
        Task<IReadOnlyList<EducationalCategory>> SearchEducationalCategoryByName(string name);

        #endregion

        #region ArticlesAdmin

        Task<IReadOnlyList<Article>> GetAllArticle();
        Task<Article> GetByIdArticle(int Id);
        Task<bool> AddArticle(Article article);
        Task<bool> UpdateArticle(Article article);
        Task<bool> DeleteArticle(int Id);
        Task<IReadOnlyList<Article>> SearchArticleByTitle(string name);

        #endregion

        #region Articles 

        //Loved
        Task<ArticleLove> GetArticleLoved(string userEmail, int articleId);
        Task<bool> AddLove(ArticleLove articleLove);
        Task<bool> IsAlreadyLoved(string userEmail, int AtricleId);
        Task<bool> DeleteLove(ArticleLove articleLove);
        
         //Save
        Task<SavedArticle> GetArticleSaved(string userEmail, int articleId);
        Task<bool> IsAlreadySaved(string userEmail, int AtricleId);
        Task<bool> AddSave(SavedArticle savedArticle);
        Task<bool> DeleteSave(SavedArticle savedArticle);
        Task<IReadOnlyList<Article>> GetAllArticleSavedForUser(string userEmail);

        //Comments
        Task<bool> AddComment(CommentsArticle commentsArticle);
        Task<bool> UpdateComment(CommentsArticle commentsArticle);
        Task<bool> DeleteComment(CommentsArticle commentsArticle);
        Task<IReadOnlyList<CommentsArticle>> GetCommentByIdArticleWithSpec(CommentArticleParams commedParams);
        Task<int> GetCountCommentWithSpec(CommentArticleParams commentParam);
        Task<CommentsArticle> GetCommentById(int commentId);

        //Article
        Task<Article> GetArticleByIdWithSpec(int Id);
        Task<IReadOnlyList<Article>> GetAllArticledWithSpec(ArticleSpecParams articleParams);
        Task<int> GeyCountArticleWithSpec(ArticleSpecParams articleParams);
        Task<IReadOnlyList<Article>> GetArticleSavedForUser(SavedArticleParams savedArticleParams, string userEmail);

        #endregion
    }
}
