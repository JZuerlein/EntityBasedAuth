using EntityBasedAuth.Auth.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace EntityBasedAuth.Auth.Policies
{
    public static class EmployeeReviewPolicies
    {
        public const string UserCanViewPolicyName = "UserCanView";
        public static AuthorizationPolicy GetUserCanViewPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .AddRequirements(new UserIsSubjectOfReviewRequirement())
                .Build();
        }

        public const string ReadReviewPolicyName = "ReadReview";
        public static AuthorizationPolicy GetReadReviewPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .AddRequirements(new UserCanViewReviewRequirement())
                .Build();
        }

        public const string SubmitReviewPolicyName = "SubmitReview";
        public static AuthorizationPolicy GetSubmitReviewPolicy()
        {
            var builder = new AuthorizationPolicyBuilder();
            builder.AddRequirements(new IAuthorizationRequirement[] {
                new UserCanSubmitReviewRequirement()
            });

            builder.RequireAuthenticatedUser();
            return builder.Build();
        }
    }
}
