using EyeCareHub.DAL.Entities.Content_Education;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace EyeCareHub.API.Dtos.ContentEducations
{
    public class ArticleDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Title is Requird")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is Requird")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Content is Requird")]
        public string Content { get; set; }
        [Required(ErrorMessage = "Category is Requird")]
        public int CategoryId { get; set; }

        public IFormFile Picture { get; set; }
    }
}
