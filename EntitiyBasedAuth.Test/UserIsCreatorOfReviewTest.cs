using Microsoft.AspNetCore.Authorization;
using EntityBasedAuth.Domain;
using System.Security.Claims;
using Xunit;
using EntityBasedAuth.Auth.Requirements;

namespace EntitiyBasedAuth.Test
{
    public class UserIsCreatorOfReviewTest : IDisposable
    {
        public UserIsCreatorOfReviewTest()
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
        public async Task HandleAsync_ReturnsSucceeded_WhenUserIsCreator()
        {
            //Arrange
            var claims = new Claim[] { new Claim("UserId", creatorId.ToString(), "string", "MyOrganization") };
            var user = GetNewClaimsPrincipal(claims);

            var authContext = new AuthorizationHandlerContext(
                new List<IAuthorizationRequirement>
                {
                    new UserCanViewReviewRequirement()
                }, user, employeeReview);

            //Act
            var sut = new UserIsCreatorOfReviewHandler<UserCanViewReviewRequirement>();
            await sut.HandleAsync(authContext);

            //Assert
            Assert.True(authContext.HasSucceeded);
        }


        [Fact]
        public async Task HandleAsync_ReturnsNotSucceeded_WhenUserIsSubject()
        {
            //Arrange
            var claims = new Claim[] { new Claim("UserId", employeeId.ToString(), "string", "MyOrganization") };
            var user = GetNewClaimsPrincipal(claims);

            var authContext = new AuthorizationHandlerContext(new List<IAuthorizationRequirement> { new UserCanViewReviewRequirement() }, user, employeeReview);

            //Act
            var sut = new UserIsCreatorOfReviewHandler<UserCanViewReviewRequirement>();
            await sut.HandleAsync(authContext);

            //Assert
            Assert.False(authContext.HasSucceeded);
        }
    }
}
