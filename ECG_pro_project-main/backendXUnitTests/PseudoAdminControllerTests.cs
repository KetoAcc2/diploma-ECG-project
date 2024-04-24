using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Moq;

namespace backendXUnitTests
{
    public class PseudoAdminControllerTests
    {
        #region UpdateRoleAsync
        [Fact]
        public async void UpdateRoleAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var returnUser = new User { Role = UserRoleType.PSEUDO_ADMIN };
            var returnUserByEmail = new User { };

            // Arrange
            var pseudoAdminService = new Mock<IPseudoAdminService>();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new PseudoAdminController(pseudoAdminService.Object) { ControllerContext = { HttpContext = httpContext } };

            pseudoAdminService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            pseudoAdminService.Setup(x => x.GetUserByEmail(It.IsAny<string>()))
                .ReturnsAsync(returnUserByEmail);

            pseudoAdminService.Setup(x => x.UpdateRole(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var response = await controller.UpdateRole(new RoleUpdateDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void UpdateRoleAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var pseudoAdminService = new Mock<IPseudoAdminService>();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new PseudoAdminController(pseudoAdminService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.UpdateRole(new RoleUpdateDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void UpdateRoleAsync_ProvidingWrongDataEmail_Returns404()
        {
            var expectedStatusCode = 404;
            var returnUser = new User { Role = UserRoleType.PSEUDO_ADMIN };

            // Arrange
            var pseudoAdminService = new Mock<IPseudoAdminService>();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new PseudoAdminController(pseudoAdminService.Object) { ControllerContext = { HttpContext = httpContext } };

            pseudoAdminService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            // Act
            var response = await controller.UpdateRole(new RoleUpdateDTO { Email = "wrong@error.com"});

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void UpdateRoleAsync_ProvidingUserByEmailWithRolePseudoAdmin_Returns403()
        {
            var expectedStatusCode = 403;
            var returnUser = new User {  };
            var returnUserByEmail = new User { Role = UserRoleType.PSEUDO_ADMIN };

            // Arrange
            var pseudoAdminService = new Mock<IPseudoAdminService>();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new PseudoAdminController(pseudoAdminService.Object) { ControllerContext = { HttpContext = httpContext } };

            pseudoAdminService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            pseudoAdminService.Setup(x => x.GetUserByEmail(It.IsAny<string>()))
                .ReturnsAsync(returnUserByEmail);

            // Act
            var response = await controller.UpdateRole(new RoleUpdateDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void UpdateRoleAsync_UnsuccessfulUpdateRole_Returns400()
        {
            var expectedStatusCode = 400;
            var returnUser = new User { Role = UserRoleType.PSEUDO_ADMIN };
            var returnUserByEmail = new User { };

            // Arrange
            var pseudoAdminService = new Mock<IPseudoAdminService>();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new PseudoAdminController(pseudoAdminService.Object) { ControllerContext = { HttpContext = httpContext } };

            pseudoAdminService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            pseudoAdminService.Setup(x => x.GetUserByEmail(It.IsAny<string>()))
                .ReturnsAsync(returnUserByEmail);

            pseudoAdminService.Setup(x => x.UpdateRole(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var response = await controller.UpdateRole(new RoleUpdateDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region GetITDocAsync
        [Fact]
        public async void GetITDocAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedString = "itdoc";

            // Arrange
            var pseudoAdminService = new Mock<IPseudoAdminService>();
            var controller = new PseudoAdminController(pseudoAdminService.Object);

            pseudoAdminService.Setup(x => x.GetITDoc())
                .ReturnsAsync(expectedString);

            // Act
            var response = await controller.GetITDoc();

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var result = Assert.IsAssignableFrom<string>(res.Value);
            Assert.Equal(expectedString, result);
        }
        #endregion
    }
}
