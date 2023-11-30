using Microsoft.AspNetCore.Authorization;
using PolicyExample.CustomExceptions;
using PolicyExample.Helpers;
using System.Text;

namespace PolicyExample.AppPolicies
{
    public class FreeAccesRequirement : IAuthorizationRequirement
    {
    }

    public class FreeAccesRequirmentHander : AuthorizationHandler<FreeAccesRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, FreeAccesRequirement requirement)
        {


            var result = context.User.HasClaim(x => x.Type == "AccessDate");


            if (!result)
            {
                context.Fail();
                return Task.CompletedTask;
            }


            var dateTime = context.User.FindFirst(x => x.Type == "AccessDate").Value;

            if (DateTime.Now > Convert.ToDateTime(dateTime))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }


    public class SearhRequirement:IAuthorizationRequirement
    {

    }

    public class SearhRequirementHandler : AuthorizationHandler<SearhRequirement>
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public SearhRequirementHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SearhRequirement requirement)
        {


           var result =  context.User.Claims.Any(x => x.Type == AppDefaults.SearchClaim);


            if (!result)
            {

                context.Fail();
                return Task.CompletedTask;

            }

            var claimValue =  context.User.FindFirst(x => x.Type == AppDefaults.SearchClaim).Value;

            //10.10.23 satın aldığı gün 15 gün geçerli
            //10.25.23 => son tarih 

            //bugün > ise son tarihden artık işlem geçersiz olacaktır

            var endDate =  Convert.ToDateTime(claimValue);

            if (DateTime.Now> endDate)
            {
                
                //var btyes =  Encoding.UTF8.GetBytes("paketiniz süresi doldu");

                //httpContextAccessor.HttpContext.Response.StatusCode = 403;
                //httpContextAccessor.HttpContext.Response.ContentType = "application/json";
                //httpContextAccessor.HttpContext.Response.Body.WriteAsync(btyes, 0, btyes.Length);

                //context.Fail()
                context.Fail();
                return Task.CompletedTask;

                //throw new FeatureNotFoundException();

            }

            context.Succeed(requirement);
            return Task.CompletedTask;



        }
    }

}
