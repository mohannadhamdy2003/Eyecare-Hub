using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EyeCareHub.API.Dtos.ContentEducations
{
    public class ArticleAddDto
    {
        [Required(ErrorMessage = "Title is Requird")]
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
