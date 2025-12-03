using AutoMapper;
using EyeCareHub.API.Dtos.Doctors;
using EyeCareHub.API.Dtos.Product;
using EyeCareHub.DAL.Entities.Identity;
using EyeCareHub.DAL.Entities.ProductInfo;
using Microsoft.Extensions.Configuration;

namespace EyeCareHub.API.Helper
{
    public class DoctorPicURLResolver : IValueResolver<Doctor, DoctorResponseDto, string>
    {
        private readonly IConfiguration _config;

        public DoctorPicURLResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Doctor source, DoctorResponseDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_config["BaseURL"]}{source.PictureUrl}";
            }

            return null;
        }
    }
}
