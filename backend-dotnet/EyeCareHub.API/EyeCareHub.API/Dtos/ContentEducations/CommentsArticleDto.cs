using EyeCareHub.DAL.Entities.Content_Education;
using EyeCareHub.DAL.Entities.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace EyeCareHub.API.Dtos.ContentEducations
{
    public class CommentsArticleDto
    {
        [Required(ErrorMessage = "Comment Is Required")]

        public string comment { get; set; }

        [Required(ErrorMessage ="ArticleId Is Required")]
        public int ArticleId { get; set; }
    }
}
