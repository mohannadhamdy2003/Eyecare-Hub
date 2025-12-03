using AutoMapper;
using AutoMapper.Configuration;
using EyeCareHub.API.Dtos.OrderDtos;
using EyeCareHub.API.Extensions;
using EyeCareHub.API.Helper;
using EyeCareHub.API.Middlewares;
using EyeCareHub.DAL.Data;
using EyeCareHub.DAL.Entities.OrderAggregate;
using EyeCareHub.DAL.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EyeCareHub.API
{
    public class Startup
    {
        public Startup(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EyeCareHub.API", Version = "v1" });
            });
            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"));
            });
            services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));   //singletone
            });

            services.AddSingleton<IConnectionMultiplexer>(S =>
            {
                var connect = Configuration.GetSection("BasketSettings:Redis").Value;

                return ConnectionMultiplexer.Connect(connect);
            });

            services.AddIdentityServices(Configuration);
            services.AddApplicationServices();

            services.AddControllers()
                .AddJsonOptions(options =>
                     {
                         options.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
                         options.JsonSerializerOptions.Converters.Add(new DateTimeOffsetConverter());
                     });

            //  services.AddScoped<IValueResolver<OrderItem, OrderItemDto, string>, ProductItemOrderPictureURLResolver>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EyeCareHub.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
