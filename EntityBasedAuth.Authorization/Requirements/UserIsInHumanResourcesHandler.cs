using Microsoft.AspNetCore.Authorization;

namespace EntityBasedAuth.Auth.Requirements
{
    public class UserIsInHumanResourcesHandler : AuthorizationHandler<UserCanViewReviewRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       UserCanViewReviewRequirement requirement)
        {
            if (context.User.IsInRole("HumanResources"))
            {
                context.Succeed(requirement);
            }

            return Task.FromResult(0);
        }
    }
}
