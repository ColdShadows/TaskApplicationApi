using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskApplicationApi.Services.Interfaces;
using User = TaskApplicationApi.Models.User;

namespace TaskApplicationApi.Controllers
{
    public class UsersControllerTests
    {
        [Fact]
        public void CtorWithNullUsersServiceThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new UsersController(null));
        }

        [Fact]
        public async Task GetSuccessReturnsExpected()
        {
            var user = new User
            {
                Id = "id",
                FirstName = "First",
                LastName = "Last",
                PreferencesId = "id",
                UserSubject = "subject"
            };

            var mockUserService = new Mock<IUsersService>();
            mockUserService
                .Setup(x => x.GetByUserSubject(It.IsAny<string>()))
                .ReturnsAsync(user);

            var subject = new UsersController(mockUserService.Object) { };
            SetClaimsPrincipal(subject);

            var result = await subject.Get();

            result.Result.Should().BeOfType<OkObjectResult>();

            var okObjectResult = result.Result as OkObjectResult;
            okObjectResult.Value.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task PostSuccessReturnsExpected()
        {
            var user = new User
            {
                Id = "id",
                FirstName = "First",
                LastName = "Last",
                PreferencesId = "id",
                UserSubject = "subject"
            };

            var mockUserService = new Mock<IUsersService>();
            mockUserService
                .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(user);

            var subject = new UsersController(mockUserService.Object) { };
            SetClaimsPrincipal(subject);

            var result = await subject.Post(user);

            result.Result.Should().BeOfType<CreatedAtActionResult>();

            var createdAtActionResult = result.Result as CreatedAtActionResult;
            createdAtActionResult.Value.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task PutSuccessReturnsExpected()
        {
            var user = new User
            {
                Id = "id",
                FirstName = "First",
                LastName = "Last",
                PreferencesId = "id",
                UserSubject = "subject"
            };

            var mockUserService = new Mock<IUsersService>();
            mockUserService
                .Setup(x => x.Update(It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(user);

            var subject = new UsersController(mockUserService.Object) { };
            SetClaimsPrincipal(subject);

            var result = await subject.Put(user);

            result.Result.Should().BeOfType<OkObjectResult>();

            var okObjectResult = result.Result as OkObjectResult;
            okObjectResult.Value.Should().BeEquivalentTo(user);
        }

        private void SetClaimsPrincipal(UsersController controller)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, "subject"),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = claimsPrincipal };
        }
    }
}
