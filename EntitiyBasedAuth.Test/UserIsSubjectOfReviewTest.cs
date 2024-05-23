using EntityBasedAuth.Auth.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using EntityBasedAuth.Domain;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EntitiyBasedAuth.Test
{
    public class UserIsSubjectOfReviewTest : IDisposable
    {
        private readonly IAuthorizationService _authorizationService;

        public UserIsSubjectOfReviewTest()
        {
            _authorizationService = BuildAuthorizationService(services =>
            {
                services.AddAuthorizationCore(options =>
                {
                    options.AddPolicy("Custom",
                        policy => policy.Requirements.Add(new UserCanViewReviewRequirement()));
                });
                services.AddSingleton<IAuthorizationHandler, UserIsSubjectOfReviewHandler<UserCanViewReviewRequirement>>();
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
        public async Task HandleAsync_ReturnsSucceeded_WhenUserIsSubject()
        {
            //Arrange
            var claims = new Claim[] { new Claim("UserId", employeeId.ToString(), "string", "MyOrganization") };
            var user = GetNewClaimsPrincipal(claims);

            var authContext = new AuthorizationHandlerContext(new List<IAuthorizationRequirement> { new UserCanViewReviewRequirement() }, user, employeeReview);

            //Act
            var sut = new UserIsSubjectOfReviewHandler<UserCanViewReviewRequirement>();
            await sut.HandleAsync(authContext);

            //Assert
            Assert.True(authContext.HasSucceeded);
        }


        [Fact]
        public async Task HandleAsync_ReturnsNotSucceeded_WhenUserIsCreator()
        {
            //Arrange
            var claims = new Claim[] { new Claim("UserId", creatorId.ToString(), "string", "MyOrganization") };
            var user = GetNewClaimsPrincipal(claims);

            var authContext = new AuthorizationHandlerContext(new List<IAuthorizationRequirement> { new UserCanViewReviewRequirement() }, user, employeeReview);

            //Act
            var sut = new UserIsSubjectOfReviewHandler<UserCanViewReviewRequirement>();
            await sut.HandleAsync(authContext);

            //Assert
            Assert.False(authContext.HasSucceeded);
        }
    }
}
