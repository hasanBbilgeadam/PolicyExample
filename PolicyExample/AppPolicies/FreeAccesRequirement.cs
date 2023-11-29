using Microsoft.AspNetCore.Authorization;

namespace PolicyExample.AppPolicies
{
    public class FreeAccesRequirement:IAuthorizationRequirement
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
}
