using EyeCareHub.BLL.Interface;
using EyeCareHub.BLL.specifications;
using EyeCareHub.BLL.specifications.ArticlesSpecifications;
using EyeCareHub.DAL.Data;
using EyeCareHub.DAL.Entities.Content_Education;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.BLL.Repositories
{
    public class ContentEducationRepo : IContentEducationRepo
    {

        private readonly IUnitOfWork<StoreContext> _unitOfWork;

        public ContentEducationRepo(IUnitOfWork<StoreContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        #region EducationalCategory
        public async Task<bool> AddEducationalCategory(EducationalCategory educationalCategory)
        {
            await _unitOfWork.Repository<EducationalCategory>().Add(educationalCategory);
            var result = await _unitOfWork.Complete();
            return result > 0;
        }



        public async Task<bool> DeleteEducationalCategory(int educationalCategoryId)
        {
            var educationalCategory = await _unitOfWork.Repository<EducationalCategory>().GetByIdAsync(educationalCategoryId);
            _unitOfWork.Repository<EducationalCategory>().Delete(educationalCategory);

            var result = await _unitOfWork.Complete();
            return result > 0;
        }



        public async Task<IReadOnlyList<EducationalCategory>> GeAllEducationalCategory()
        {
            return await _unitOfWork.Repository<EducationalCategory>().GetAllAsync();
        }



        public async Task<EducationalCategory> GetByIdEducationalCategory(int Id)
        {
            return await _unitOfWork.Repository<EducationalCategory>().GetByIdAsync(Id);
        }



        public async Task<IReadOnlyList<EducationalCategory>> SearchEducationalCategoryByName(string name)
        {
            return await _unitOfWork.Repository<EducationalCategory>().FindAsync(c => c.Name.ToLower().Contains(name.ToLower()));
        }



        public async Task<bool> UpdateEducationalCategory(EducationalCategory educationalCategory)
        {
            _unitOfWork.Repository<EducationalCategory>().Update(educationalCategory);

            var result = await _unitOfWork.Complete();

            return result > 0;
        }

        #endregion


        #region ArticlesAdmin
        public async Task<bool> AddArticle(Article article)
        {
            await _unitOfWork.Repository<Article>().Add(article);
            var result = await _unitOfWork.Complete();
            return result > 0;
        }


        public async Task<bool> DeleteArticle(int Id)
        {
            var educationalCategory = await _unitOfWork.Repository<Article>().GetByIdAsync(Id);
            _unitOfWork.Repository<Article>().Delete(educationalCategory);

            var result = await _unitOfWork.Complete();
            return result > 0;
        }

        public async Task<IReadOnlyList<Article>> GetAllArticle()
        {
            return await _unitOfWork.Repository<Article>().GetAllAsync();
        }

        public async Task<Article> GetByIdArticle(int Id)
        {
            return await _unitOfWork.Repository<Article>().GetByIdAsync(Id);
        }
        public async Task<IReadOnlyList<Article>> SearchArticleByTitle(string name)
        {
            return await _unitOfWork.Repository<Article>().FindAsync(c => c.Title.ToLower().Contains(name.ToLower()));
        }

        public async Task<bool> UpdateArticle(Article article)
        {

            _unitOfWork.Repository<Article>().Update(article);

            var result = await _unitOfWork.Complete();

            return result > 0;
        }

        #endregion

        #region Articles

        // Loved Article
        public async Task<ArticleLove> GetArticleLoved( string userEmail, int articleId)
        {
            // await _context.Set<ArticleLove>().FirstOrDefaultAsync(al => al.UserEmail == userEmail && al.ArticleId == articleId);
            var data=  await _unitOfWork.Repository<ArticleLove>().FindAsync(al => al.UserEmail == userEmail && al.ArticleId == articleId);
            return data.FirstOrDefault();
            //return await _unitOfWork.Repository<ArticleLove>().GetByIdAsync(AtricleId);
        }
        public async Task<bool> AddLove(ArticleLove articleLove)
        {
            await _unitOfWork.Repository<ArticleLove>().Add(articleLove);
            var result = await _unitOfWork.Complete();
            return result > 0;
        }
        public async Task<bool> IsAlreadyLoved(string userEmail,int AtricleId)
        {
            var result  = await GetArticleLoved( userEmail, AtricleId);
            return result != null;
        }
        public async Task<bool> DeleteLove(ArticleLove articleLove)
        {
            _unitOfWork.Repository<ArticleLove>().Delete(articleLove);
            var result = await _unitOfWork.Complete();
            return result > 0;
        }
        

        // Saved Article
        public async Task<SavedArticle> GetArticleSaved( string userEmail, int articleId)
        {
            var data = await _unitOfWork.Repository<SavedArticle>().FindAsync(al => al.UserEmail == userEmail && al.ArticleId == articleId);
            return data.FirstOrDefault();
        }
        public async Task<bool> AddSave(SavedArticle savedArticle)
        {
            await _unitOfWork.Repository<SavedArticle>().Add(savedArticle);
            var result = await _unitOfWork.Complete();
            return result > 0;
        }
        public async Task<bool> IsAlreadySaved(string userEmail, int AtricleId)
        {
            var result = await GetArticleSaved( userEmail, AtricleId);
            return result != null;
        }
        public async Task<bool> DeleteSave(SavedArticle savedArticle)
        {
            _unitOfWork.Repository<SavedArticle>().Delete(savedArticle);
            var result = await _unitOfWork.Complete();
            return result > 0;
        }

        public async Task<IReadOnlyList<Article>> GetAllArticleSavedForUser(string userEmail)
        {
            return await _unitOfWork.Repository<Article>().FindAsync(al => al.SavedArticles.Any(sa => sa.UserEmail == userEmail));
        }


        // Comments For Article
        public async Task<bool> AddComment(CommentsArticle commentsArticle)
        {
            await _unitOfWork.Repository<CommentsArticle>().Add(commentsArticle);
            var result = await _unitOfWork.Complete();
            return result > 0;
        }
        public async Task<bool> UpdateComment(CommentsArticle commentsArticle)
        {
            _unitOfWork.Repository<CommentsArticle>().Update(commentsArticle);
            var result = await _unitOfWork.Complete();
            return result > 0;
        }
        public async Task<bool> DeleteComment(CommentsArticle commentsArticle)
        {
            _unitOfWork.Repository<CommentsArticle>().Delete(commentsArticle);
            var result = await _unitOfWork.Complete();
            return result > 0;
        }

        public async Task<IReadOnlyList<CommentsArticle>> GetCommentByIdArticleWithSpec(CommentArticleParams commentParam)
        {
            var spec = new CommentArticleSpec(commentParam);

            return await _unitOfWork.Repository<CommentsArticle>().GetAllWithspecAsync(spec);
        }

        public async Task<int> GetCountCommentWithSpec(CommentArticleParams commentParam)
        {
            var spec = new CommentArticleSpec(commentParam);

            return await _unitOfWork.Repository<CommentsArticle>().GetCountAsync(spec);
        }

        public async Task<CommentsArticle> GetCommentById(int commentId)
        {
            return await _unitOfWork.Repository<CommentsArticle>().GetByIdAsync(commentId);
        }



        // Get Article
        public async Task<IReadOnlyList<Article>> GetAllArticledWithSpec(ArticleSpecParams articleParams)
        {
            var spec = new ArticlesWithCategorySpec(articleParams);
            var data = await _unitOfWork.Repository<Article>().GetAllWithspecAsync(spec);
            return data;
        }
        public async Task<int> GeyCountArticleWithSpec(ArticleSpecParams articleParams)
        {
            var spec = new CountArticleSpec(articleParams);
            var data = await _unitOfWork.Repository<Article>().GetAllWithspecAsync(spec);
            return data.Count();
        }
        public async Task<Article> GetArticleByIdWithSpec(int Id)
        {
            var spec = new ArticlesWithCategorySpec(Id);
            var data = await _unitOfWork.Repository<Article>().GetByIdWithspecAsync(spec);
            return data;
        }


        public async Task<IReadOnlyList<Article>> GetArticleSavedForUser(SavedArticleParams savedArticleParams, string userEmail)
        {
            var spec =new SavedArticleSpec(savedArticleParams, userEmail);

            return await _unitOfWork.Repository<Article>().GetAllWithspecAsync(spec);


        }



        #endregion


    }
}
