
using EntityBasedAuth.Auth.Policies;
using EntityBasedAuth.Auth.Requirements;
using EntityBasedAuth.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Xunit;


namespace EntitiyBasedAuth.Test
{
    public class EmployeeReviewPolicyTest : IDisposable
    {
        private readonly IAuthorizationService _authorizationService;

        public EmployeeReviewPolicyTest()
        {
            _authorizationService = BuildAuthorizationService(services =>
            {
                services.AddAuthorizationCore(options =>
                {
                    options.AddPolicy(EmployeeReviewPolicies.ReadReviewPolicyName,
                                      EmployeeReviewPolicies.GetReadReviewPolicy());
                    options.AddPolicy(EmployeeReviewPolicies.SubmitReviewPolicyName,
                                      EmployeeReviewPolicies.GetSubmitReviewPolicy());
                });

                services.AddSingleton<IAuthorizationHandler, UserIsCreatorOfReviewHandler<UserCanViewReviewRequirement>>();
                services.AddSingleton<IAuthorizationHandler, UserIsSubjectOfReviewHandler<UserCanViewReviewRequirement>>();
                services.AddSingleton<IAuthorizationHandler, UserIsInHumanResourcesHandler>();

                services.AddSingleton<IAuthorizationHandler, UserIsCreatorOfReviewHandler<UserCanSubmitReviewRequirement>>();
            });
        }

        private IAuthorizationService BuildAuthorizationService(Action<IServiceCollection> setupServices = null)
        {
            var services = new ServiceCollection();
            services.AddAuthorizationCore();
            services.AddLogging();
            services.AddOptions();
            setupServices?.Invoke(services);
            return services.BuildServiceProvider().GetRequiredService<IAuthorizationService>();
        }

        public void Dispose()
        {
        }

        private ClaimsPrincipal GetNewClaimsPrincipal(Claim[] claims)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(claims, "Custom"));
        }

        private const int employeeId = 1;
        private const int creatorId = 2;
        private const int humanResourceId = 3;
        private EmployeeReview employeeReview = new EmployeeReview(creatorId, employeeId, "TestName", 1);


        [Fact]
        public async Task AuthorizeAsync_ReturnsSucceed_WhenUserIsCreator()
        {
            //Arrange
            var claims = new Claim[] { new Claim("UserId", creatorId.ToString(), "string", "MyOrganization") };
            var user = GetNewClaimsPrincipal(claims);

            //Act
            var allowed = await _authorizationService.AuthorizeAsync(user, employeeReview, EmployeeReviewPolicies.ReadReviewPolicyName);

            //Assert
            Assert.True(allowed.Succeeded);
        }

        [Fact]
        public async Task AuthorizeAsync_ReturnsSucceed_WhenUserIsSubject()
        {
            //Arrange
            var claims = new Claim[] { new Claim("UserId", employeeId.ToString(), "string", "MyOrganization") };
            var user = GetNewClaimsPrincipal(claims);

            //Act
            var allowed = await _authorizationService.AuthorizeAsync(user, employeeReview, EmployeeReviewPolicies.ReadReviewPolicyName);

            //Assert
            Assert.True(allowed.Succeeded);
        }

        [Fact]
        public async Task AuthorizeAsync_ReturnsSucceed_WhenUserIsInHumanResources()
        {
            //Arrange
            var claims = new Claim[] { new Claim("UserId", humanResourceId.ToString(), "string", "MyOrganization"),
                                       new Claim(ClaimTypes.Role, "HumanResources")};

            var user = GetNewClaimsPrincipal(claims);


            //Act
            var allowed = await _authorizationService.AuthorizeAsync(user, employeeReview, EmployeeReviewPolicies.ReadReviewPolicyName);

            //Assert
            Assert.True(allowed.Succeeded);
        }

        [Fact]
        public async Task AuthorizeAsyncOfSubmitReviewPolicy_ReturnsSucceed_WhenUserIsCreator()
        {
            //Arrange
            var claims = new Claim[] { new Claim("UserId", creatorId.ToString(), "string", "MyOrganization") };
            var user = GetNewClaimsPrincipal(claims);

            //Act
            var allowed = await _authorizationService.AuthorizeAsync(user, employeeReview, EmployeeReviewPolicies.SubmitReviewPolicyName);

            //Assert
            Assert.True(allowed.Succeeded);
        }
    }
}
