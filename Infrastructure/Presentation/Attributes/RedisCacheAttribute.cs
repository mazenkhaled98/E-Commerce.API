using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstraction.Contracts;
using System.Text;

namespace Presentation.Attributes
{
    internal class RedisCacheAttribute(int durationInSeconds = 120) : ActionFilterAttribute
    {
        public async override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().CasheService;
            //Data cached or not ==> key
            //Key ==> Path url + query string
            // /api/Products?sort=NameDesc&pageSize=10&pageIndex=1
            //context.HttpContext.Request.Path // /api/products
            //context.HttpContext.Request.Query // Key , value ==> sort ==> NameDesc , pageSize ==> 10 , pageIndex ==> 1
            string key = GenerateKey(context.HttpContext.Request);
            var result = await cacheService.GetCachedValueAsync(key);
            if (result != null)
            {
                context.Result = new ContentResult
                {
                    Content = result,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            var resultContext = await next.Invoke();
            if (resultContext.Result is OkObjectResult okObjResult)
            {
                await cacheService.SetCacheValueAsync(key, okObjResult, TimeSpan.FromSeconds(durationInSeconds));
            }

        }
        private string GenerateKey(HttpRequest request)
        {
            //string variable ==> add path /api/products
            //variable ==> add query string values
            var key = new StringBuilder();
            key.Append(request.Path); // /api/products
                                      // /api/Products?sort=NameDesc&pageSize=10&pageIndex=1
                                      // /api/Products?pageSize=10&pageIndex=1&sort=NameDesc
                                      // /api/Products?pageSize=10&sort=NameDesc&pageIndex=1

            foreach (var item in request.Query.OrderBy(x => x.Key))
            {
                // /api/Products?pageSize-10-sort-NameDesc-pageIndex-1
                key.Append($"${item.Key}-{item.Value}");
            }

            return key.ToString();
        }
    }
}
