using AutoMapper;
using EyeCareHub.API.Dtos.ContentEducations;
using EyeCareHub.API.Dtos.Product;
using EyeCareHub.DAL.Entities.Content_Education;
using EyeCareHub.DAL.Entities.ProductInfo;
using Microsoft.Extensions.Configuration;

namespace EyeCareHub.API.Helper
{
    public class ArticleUrlPicAdmin : IValueResolver<Article, ArticleResponseAdminDto, string>
    {
        private readonly IConfiguration _config;

        public ArticleUrlPicAdmin(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Article source, ArticleResponseAdminDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_config["BaseURL"]}{source.PictureUrl}";
            }

            return null;
        }
    }
}
