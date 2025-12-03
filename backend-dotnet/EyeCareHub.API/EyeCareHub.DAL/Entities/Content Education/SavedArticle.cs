using EyeCareHub.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Entities.Content_Education
{
    public class SavedArticle:BaseEntity
    {
        public string UserEmail { get; set; } 
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public DateTime SavedAt { get; set; }
    }
}