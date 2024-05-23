using EntityBasedAuth.Domain;
using Microsoft.AspNetCore.Authorization;

namespace EntityBasedAuth.Auth.Requirements
{
    public class UserIsCreatorOfReviewHandler<TRequirement> : AuthorizationHandler<TRequirement, EmployeeReview>
        where TRequirement : IAuthorizationRequirement

    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                              TRequirement requirement,
                                              EmployeeReview employeeReview)
        {
            var userIdString = context.User.Claims.FirstOrDefault(_ => _.Issuer == "MyOrganization" &&
                                                                       _.Type == "UserId")?.Value;

            int userId;

            if (int.TryParse(userIdString, out userId) &&
                employeeReview != null &&
                userId == employeeReview.CreatorId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

        }
    }
}
