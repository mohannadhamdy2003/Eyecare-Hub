using AutoMapper;
using EyeCareHub.API.Dtos.ContentEducations;
using EyeCareHub.DAL.Entities.Content_Education;
using Microsoft.Extensions.Configuration;

namespace EyeCareHub.API.Helper
{
    public class ArticleUrlPicUser : IValueResolver<Article, ArticleResponseUserDto, string>
    {
        private readonly IConfiguration _config;

        public ArticleUrlPicUser(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Article source, ArticleResponseUserDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_config["BaseURL"]}{source.PictureUrl}";
            }

            return null;
        }
    }
}
