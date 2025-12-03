using System;

namespace EyeCareHub.API.Dtos.ContentEducations
{
    public class CommentToReturnDto
    {
        public string comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }
    }
}
