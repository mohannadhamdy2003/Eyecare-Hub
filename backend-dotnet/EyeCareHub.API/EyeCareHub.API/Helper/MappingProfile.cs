using AutoMapper;
using EyeCareHub.API.Dtos.Appointment;
using EyeCareHub.API.Dtos.AuthDtos;
using EyeCareHub.API.Dtos.ContentEducations;
using EyeCareHub.API.Dtos.DiagnosisHistory;
using EyeCareHub.API.Dtos.Doctors;
using EyeCareHub.API.Dtos.MedicalRecord;
using EyeCareHub.API.Dtos.OrderDtos;
using EyeCareHub.API.Dtos.Product;
using EyeCareHub.DAL.Entities.Content_Education;
using EyeCareHub.DAL.Entities.Identity;
using EyeCareHub.DAL.Entities.OrderAggregate;
using EyeCareHub.DAL.Entities.ProductInfo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EyeCareHub.API.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductBrands,BrandDto>().ReverseMap();
            CreateMap<ProductTypes, TypeDto>().ReverseMap();
            CreateMap<ProductCategory, CategoryDto>().ReverseMap();


            CreateMap<PatientRegistrationDto, Patient>();

            CreateMap<Products, ProductToReturn>()
                .ForMember(d => d.Brand, O => O.MapFrom(S => S.ProductBrand))
                .ForMember(d => d.Type, O => O.MapFrom(S => S.ProductType.Name))
                .ForMember(d => d.Category, O => O.MapFrom(S => S.ProductCategory.Name))
                .ForMember(dest => dest.PictureUrl,
                        opt => opt.MapFrom<ProductPictureURLResolver>());


            CreateMap<Products, UpdateProductDto>();



            CreateMap<AddresDto, Address>().ReverseMap();

            CreateMap<EducationalCategory, EducationalCategoryDto>().ReverseMap();
            CreateMap<EducationalCategory, EducationalCategoryAddDto>().ReverseMap();


            CreateMap<Article, ArticleAddDto>().ReverseMap();
            CreateMap<Article, ArticleDto>().ReverseMap();

            CreateMap<Article, ArticleResponseAdminDto>()
                .ForMember(dest => dest.PictureUrl,
                        opt => opt.MapFrom<ArticleUrlPicAdmin>());

            CreateMap<Article, ArticleResponseUserDto>()
                .ForMember(dest => dest.PictureUrl,
                        opt => opt.MapFrom<ArticleUrlPicUser>());

            CreateMap<DoctorWorkScheduleDto, DoctorWorkSchedule>()
                .ForMember(dest => dest.WorkDays, opt => opt.MapFrom<WorkDaysFromDtoResolver>());


            CreateMap<DoctorWorkSchedule, DoctorWorkScheduleDto>()
                .ForMember(dest => dest.WorkDays, opt => opt.MapFrom<WorkDaysToDtoResolver>());




            CreateMap<Doctor, DoctorResponseDto>()
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.DoctorWorkSchedule.StartTime))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.DoctorWorkSchedule.EndTime))

            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.DoctorWorkSchedule != null ? src.DoctorWorkSchedule.StartTime : TimeSpan.Zero))

            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.DoctorWorkSchedule != null ? src.DoctorWorkSchedule.EndTime : TimeSpan.Zero))
            .ForMember(dest => dest.PictureUr, opt => opt.MapFrom<DoctorPicURLResolver>());

            CreateMap<DoctorWorkSchedule, DoctorWorkScheduleDto>()
                .ForMember(dest => dest.WorkDays, opt => opt.MapFrom(src =>
                    Enum.GetValues(typeof(WorkDays))
                        .Cast<WorkDays>()
                        .Where(day => day != WorkDays.None && (src.WorkDays & day) == day)
                        .Select(day => day.ToString())
                        .ToList()));


            //AddresDto

            CreateMap<DoctorInfoDto, AppUser>()
            .ForMember(dest => dest.DisplyName, opt => opt.MapFrom(src => src.DisplyName))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            // من DTO لـ Doctor
            CreateMap<DoctorInfoDto, Doctor>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DisplyName))
                .ForMember(dest => dest.ClinicAddress, opt => opt.MapFrom(src => src.ClinicAddress))
                .ForMember(dest => dest.YearsOfExperience, opt => opt.MapFrom(src => src.YearsOfExperience))
                .ForMember(dest => dest.ConsultationFee, opt => opt.MapFrom(src => src.ConsultationFee));


            CreateMap<AppointmentDto, Appointment>()
                .ForMember(dest => dest.WorkDays, opt => opt.MapFrom(src => Enum.Parse<WorkDays>(src.WorkDays, true)));

            CreateMap<Appointment, AppointmentResponsePatientDto>()
                .ForMember(dest => dest.WorkDays, opt => opt.MapFrom(src => src.WorkDays.ToString()))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.Name));

            CreateMap<Appointment, AppointmentResponseDoctorDto>()
               .ForMember(dest => dest.WorkDays, opt => opt.MapFrom(src => src.WorkDays.ToString()))
               .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient));

            CreateMap<MedicalRecord, MedicalRecordDto>().ReverseMap();

            CreateMap<CommentsArticle,CommentsArticleDto>().ReverseMap();
            CreateMap<CommentsArticle, CommentToReturnDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));

            // Order AddressDto

            CreateMap<Order, OrderToReturnDto>();
            CreateMap<AddresDto, AddressOrder>().ReverseMap();
            CreateMap<DeliveryMethod, DeliveryMethodDto>();

            CreateMap<Order, OrderDto>().ReverseMap();


            CreateMap<ProductItemOrder, ProductItemOrderDto>();
            CreateMap<DeliveryMethod, DeliveryMethodToAddDto>();

            //Diagnosis
            CreateMap<DiagnosisHistory, DiagnosisAIResponse>();
            CreateMap<DiagnosisHistory, DiagnosisAIDto>().ReverseMap();
        }
        private static WorkDays ConvertWorkDaysToEnum(List<string> workDays)
        {
            if (workDays == null || !workDays.Any())
                return WorkDays.None;

            WorkDays result = WorkDays.None;
            foreach (var day in workDays)
            {
                if (Enum.TryParse<WorkDays>(day, true, out var workDay))
                {
                    result |= workDay;
                }
            }
            return result;
        }
    }
}
