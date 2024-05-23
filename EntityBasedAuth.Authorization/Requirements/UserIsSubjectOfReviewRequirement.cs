using EntityBasedAuth.Domain;
using Microsoft.AspNetCore.Authorization;

namespace EntityBasedAuth.Auth.Requirements
{
    public class UserIsSubjectOfReviewRequirement :
        AuthorizationHandler<UserIsSubjectOfReviewRequirement, EmployeeReview>,
        IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       UserIsSubjectOfReviewRequirement requirement,
                                                       EmployeeReview employeeReview)
        {
            var userIdString = context.User.Claims.FirstOrDefault(_ => _.Issuer == "MyOrganization" &&
                                                                       _.Type == "UserId")?.Value;

            int userId;

            if (int.TryParse(userIdString, out userId) &&
                employeeReview != null &&
                userId == employeeReview.EmployeeId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

        }
    }
}
