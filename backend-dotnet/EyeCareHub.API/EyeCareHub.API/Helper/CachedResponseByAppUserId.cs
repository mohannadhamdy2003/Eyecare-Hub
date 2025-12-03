using EyeCareHub.BLL.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.API.Helper
{
    public class CachedResponseByAppUserId : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;

        public CachedResponseByAppUserId(int cachedToLiveInSeconds)
        {
            _timeToLiveInSeconds = cachedToLiveInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            var cacheKey = GenerateCacheKey(context.HttpContext.Request);

            var cacheResponse = await cacheService.GetCachedResponse(cacheKey);

            if (!string.IsNullOrEmpty(cacheResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200,
                };
                context.Result = contentResult;
                return;
            }

            var executedEndPointContext = await next();
            if (executedEndPointContext.Result is OkObjectResult okObjectResult)
                await cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
        }



        private string GenerateCacheKey(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }

            var userId = request.HttpContext.User.Identity.IsAuthenticated
                ? request.HttpContext.User.Identity.Name // أو أي Claim فيه ID
                : "anonymous";

            keyBuilder.Append($"|user-{userId}");

            return keyBuilder.ToString();
        }
    }
}
