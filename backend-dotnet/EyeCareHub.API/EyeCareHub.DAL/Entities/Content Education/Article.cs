using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Entities.Content_Education
{
    public class Article : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string PictureUrl { get; set; }
        public int LovesCount { get; set; }
        public DateTime PublishedDate { get; set; }
        public int CategoryId { get; set; }
        public EducationalCategory Category { get; set; }
        public List<SavedArticle> SavedArticles { get; set; } = new List<SavedArticle>();
        public List<ArticleLove> ArticleLoves { get; set; } = new List<ArticleLove>();
        public int CommentsArticleId { get; set; }
        public List<CommentsArticle> commentsArticle { get; set; }
    }
}
