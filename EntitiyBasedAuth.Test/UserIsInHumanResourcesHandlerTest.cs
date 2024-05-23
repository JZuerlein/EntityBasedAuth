using EntityBasedAuth.Auth.Requirements;
using Microsoft.AspNetCore.Authorization;
using EntityBasedAuth.Domain;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EntitiyBasedAuth.Test
{
    public class UserIsInHumanResourcesHandlerTest : IDisposable
    {
        public UserIsInHumanResourcesHandlerTest()
        {
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
        public async Task HandleAsync_ReturnsSucceeded_WhenUserIsInHumanResourcesRole()
        {
            //Arrange
            var claims = new Claim[] { new Claim(ClaimTypes.Role, "HumanResources") };
            var user = GetNewClaimsPrincipal(claims);

            var authContext = new AuthorizationHandlerContext(
                new List<IAuthorizationRequirement>
                {
                    new UserCanViewReviewRequirement()
                }, user, employeeReview);

            //Act
            var sut = new UserIsInHumanResourcesHandler();
            await sut.HandleAsync(authContext);

            //Assert
            Assert.True(authContext.HasSucceeded);
        }


        [Fact]
        public async Task HandleAsync_ReturnsNotSucceeded_WhenUserIsNotInHumanResourcesRole()
        {
            //Arrange
            var claims = new Claim[] { };
            var user = GetNewClaimsPrincipal(claims);

            var authContext = new AuthorizationHandlerContext(new List<IAuthorizationRequirement> { new UserCanViewReviewRequirement() }, user, employeeReview);

            //Act
            var sut = new UserIsInHumanResourcesHandler();
            await sut.HandleAsync(authContext);

            //Assert
            Assert.False(authContext.HasSucceeded);
        }
    }
}
