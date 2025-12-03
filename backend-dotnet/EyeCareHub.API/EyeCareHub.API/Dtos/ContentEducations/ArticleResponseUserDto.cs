using System;

namespace EyeCareHub.API.Dtos.ContentEducations
{
    public class ArticleResponseUserDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string PictureUrl { get; set; }
        public bool IsLoved { get; set; }
        public bool IsSaved { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
