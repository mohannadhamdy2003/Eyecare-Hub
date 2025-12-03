using EyeCareHub.API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace EyeCareHub.API.Middlewares
{
    public class ExceptionMiddleware
    {
        public RequestDelegate next { get; }
        public ILogger<string> ILogger { get; }
        public IHostEnvironment env { get; }
        public ExceptionMiddleware(RequestDelegate next, ILogger<string> ILogger, IHostEnvironment env)
        {
            this.next = next;
            this.ILogger = ILogger;
            this.env = env;
        }

        public async Task InvokeAsync(HttpContext context) // اسم الفانكشن مهم 
        {
            try
            {
                await next.Invoke(context); // لو مافيش اكسبشن هيخش علي الخطوه الي بعده عادي
            }
            catch (Exception Ex) // لو في اكسبشن هيخش هنا بقا 
            {
                ILogger.LogError(Ex, Ex.Message); // علي شان يظهر في الكونصول

                context.Response.ContentType = "application/json"; // حددنا نوع الابجيكت الي هيرجع
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var ResponseMessage = env.IsDevelopment()  // هنشوف لو احنا في مرحله الدفولب 
                                          ? new ApiExcptionResponse((int)HttpStatusCode.InternalServerError, Ex.Message, Ex.StackTrace.ToString()) // هنبعت الخطا بالتفاصيل بتعته
                                          : new ApiExcptionResponse((int)HttpStatusCode.InternalServerError); // لو مش في مرحله الديفلب ف احنا هنبعت الاستاتس كود بس الكود فقط

                //var options = new JsonSerializerOptions() {PropertyNameCaseInsensitive = true}; 
                var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }; //هيخلي key  علي شكل المل كيس اول حرف مش كبتل والباقي كبتل
                var Json = JsonSerializer.Serialize(ResponseMessage, options);

                await context.Response.WriteAsync(Json);
            }
        }
    }
}
