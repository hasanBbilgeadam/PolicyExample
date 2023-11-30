using PolicyExample.Context;
using PolicyExample.CustomExceptions;
using System.Text.Json;

namespace PolicyExample.CustomMiddlewares
{
    public class CustomErrorMiddleware
    {

        private readonly RequestDelegate _next;

        public CustomErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            try
            {
                await _next(context);
            }
            catch (FeatureNotFoundException e)
            {

                context.Response.StatusCode = 403;

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(JsonSerializer.Serialize("paketinizin süresi bitti, tekrar paket alırsanız buraya erişebilirisniz"));
            }      
            catch (Exception e)
            {

                //hata olması durumunda log atacağız


                var dbContext = context.RequestServices.GetService<AppDbContext>();


                var message = e.Message;
                var path = context.Request.Path;
                var method = context.Request.Method;


                dbContext!.Logs.Add(new()
                {

                    Method = method,
                    ErrorMessage = message,
                    Path = path,

                });
                dbContext.SaveChanges();


                context.Response.StatusCode = 500;

                context.Response.ContentType="application/json";    
                
                await context.Response.WriteAsJsonAsync(JsonSerializer.Serialize("Bir hata oldu daha sonra tekrar deneryiniz"));

            }    

        }
    }
}
