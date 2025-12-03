using System.ComponentModel.DataAnnotations;

namespace EyeCareHub.API.Dtos.ContentEducations
{
    public class UpdateCommentsArticleDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Comment Is Required")]
        public string comment { get; set; }
    }
}
