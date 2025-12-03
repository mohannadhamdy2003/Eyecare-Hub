using AutoMapper;
using EyeCareHub.API.Errors;
using EyeCareHub.API.Helper;
using EyeCareHub.BLL.Interface;
using EyeCareHub.BLL.Repositories;
using EyeCareHub.DAL.Data;
using EyeCareHub.DAL.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace EyeCareHub.API.Extensions
{
    public static class ApplicationServicesExtention
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();

            services.AddScoped(typeof(ITokenService), typeof(TokenService));
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddScoped<ProductItemOrderPictureURLResolver>();

            services.AddScoped(typeof(IOrderService), typeof(OrderService));
            services.AddScoped<IContentEducationRepo, ContentEducationRepo>();
            services.AddScoped<IUnitOfWork<StoreContext>, UnitOfWork<StoreContext>>();
            services.AddScoped<IUnitOfWork<AppIdentityDbContext>, UnitOfWork<AppIdentityDbContext>>();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.Where(M => M.Value.Errors.Count > 0)
                                              .SelectMany(M => M.Value.Errors)
                                              .Select(D => D.ErrorMessage).ToArray();
                    var ResponseMessage = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(ResponseMessage);
                };
            });
            services.AddScoped<IDoctorRepo, DoctorRepo>();
            services.AddScoped<IAdminRepo,AdminRepo>();
            services.AddScoped<IAppointmentRepo,AppointmentRepo>();
            services.AddScoped<IMedicalRecordRepo, MedicalRecordRepo>();
            services.AddScoped<IDiagnosisRepo, DiagnosisRepo>();
            services.AddScoped<INotificationRepository, NotificationRep>();
            services.AddScoped<IStoreAdminRepo, StoreAdminRepo>();

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
                });

            return services;
        }
    }
}
