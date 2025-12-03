using EyeCareHub.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Entities.Content_Education
{
    public class CommentsArticle:BaseEntity
    {
        public string comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int ArticleId { get; set; }
        public Article article { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
