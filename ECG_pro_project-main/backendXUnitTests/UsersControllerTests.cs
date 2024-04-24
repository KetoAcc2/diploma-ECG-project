using backend.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Moq;

namespace backendXUnitTests
{
    public class UsersControllerTests
    {
        #region GetUsersAsync
        [Fact]
        public async void GetUsersAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedValue = new List<UserDTO> { new UserDTO { } };

            // Arrange
            var userService = new Mock<IUserService>();
            userService.Setup(x => x.GetUsersAsync())
                .ReturnsAsync(expectedValue);

            // Act
            var controller = new UsersController(userService.Object, null!);
            var response = await controller.GetUsers();

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualList = Assert.IsAssignableFrom<List<UserDTO>>(res.Value);
            Assert.Single(actualList);
            Assert.Equal(expectedValue[0], actualList[0]);
        }
        #endregion

        #region GetDashboardAsync
        [Fact]
        public async void GetDashboardAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var returnValue = new User { Id = 0, Email = "email@test.com", Role = "testrole" };
            var expectedValue = new UserDTO { UserId = 0, Email = "email@test.com", Role = "testrole" };

            // Arrange
            var userService = new Mock<IUserService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new UsersController(userService.Object, null!) { ControllerContext = { HttpContext = httpContext } };

            userService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnValue);

            // Act
            var response = await controller.GetDashboard();

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);

            var actualValue = Assert.IsAssignableFrom<UserDTO>(res.Value);
            Assert.Multiple(() =>
            {
                Assert.Equal(expectedValue.UserId, actualValue.UserId);
                Assert.Equal(expectedValue.Email, actualValue.Email);
                Assert.Equal(expectedValue.Role, actualValue.Role);
            });
        }

        [Fact]
        public async void GetDashboardAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var userService = new Mock<IUserService>();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new UsersController(userService.Object, null!) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.GetDashboard();

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region GetUserAsync
        [Fact]
        public async void GetUserAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var returnUserDTO = new UserDTO { Email = "email@test.com" };
            var returnCurrentUser = new User { Role = "Teacher", Email = "email@test.com" };
            var expectedValue = returnUserDTO;

            // Arrange
            var userService = new Mock<IUserService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new UsersController(userService.Object, null!) { ControllerContext = { HttpContext = httpContext } };

            userService.Setup(x => x.GetUserAsync(It.IsAny<int>()))
                .ReturnsAsync(returnUserDTO);

            userService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnCurrentUser);

            // Act
            var response = await controller.GetUser(0);

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var user = Assert.IsAssignableFrom<UserDTO>(res.Value);
            Assert.Equal(expectedValue, user);
        }

        [Fact]
        public async void GetUserAsync_ProvidingNotTeacher_Returns401()
        {
            var expectedStatusCode = 401;
            var returnUserDTO = new UserDTO { Email = "otherstudent@test.com" };
            var returnCurrentUser = new User { Role = "Student", Email = "student@test.com" };

            // Arrange
            var userService = new Mock<IUserService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new UsersController(userService.Object, null!) { ControllerContext = { HttpContext = httpContext } };

            userService.Setup(x => x.GetUserAsync(It.IsAny<int>()))
                .ReturnsAsync(returnUserDTO);

            userService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnCurrentUser);

            // Act
            var response = await controller.GetUser(0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void GetUserAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var userService = new Mock<IUserService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new UsersController(userService.Object, null!) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.GetUser(0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region GetUsersFromGroup
        [Fact]
        public async void GetUsersFromGroupAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedValue = new List<UserDTO> { new UserDTO { } };

            // Arrange
            var userService = new Mock<IUserService>();

            var controller = new UsersController(userService.Object, null!);
            userService.Setup(x => x.ExistsGroupAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            userService.Setup(x => x.GetUsersFromGroupAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedValue);

            // Act
            var response = await controller.GetUsersFromGroup(0);

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualList = Assert.IsAssignableFrom<List<UserDTO>>(res.Value);
            Assert.Single(actualList);
            Assert.Equal(expectedValue[0], actualList[0]);

        }

        [Fact]
        public async void GetUsersFromGroupAsync_ProvidingWrongGroupId_Returns404()
        {
            var expectedStatusCode = 404;

            // Arrange
            var userService = new Mock<IUserService>();

            var controller = new UsersController(userService.Object, null!);

            // Act
            var response = await controller.GetUsersFromGroup(0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);

        }
        #endregion

        #region GetTeacherDocAsync
        [Fact]
        public async void GetTeacherDocAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedString = "teacherdoc";
            var returnValue = new User { Role = UserRoleType.TEACHER };

            // Arrange
            var userService = new Mock<IUserService>();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new UsersController(userService.Object, null!) { ControllerContext = { HttpContext = httpContext } };

            userService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnValue);

            userService.Setup(x => x.GetTeacherDoc())
                .ReturnsAsync(expectedString);

            // Act
            var response = await controller.GetTeacherDoc();

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var result = Assert.IsAssignableFrom<string>(res.Value);
            Assert.Equal(expectedString, result);
        }

        [Fact]
        public async void GetTeacherDocAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var userService = new Mock<IUserService>();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new UsersController(userService.Object, null!) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.GetTeacherDoc();

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void GetTeacherDocAsync_ProvidingNotTeacher_Returns403()
        {
            var expectedStatusCode = 403;
            var returnValue = new User { Role = UserRoleType.STUDENT };

            // Arrange
            var userService = new Mock<IUserService>();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new UsersController(userService.Object, null!) { ControllerContext = { HttpContext = httpContext } };

            userService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnValue);

            // Act
            var response = await controller.GetTeacherDoc();

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region GetStudentDocAsync
        [Fact]
        public async void GetStudentDocAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedString = "studentdoc";

            // Arrange
            var userService = new Mock<IUserService>();
            var controller = new UsersController(userService.Object, null);

            userService.Setup(x => x.GetStudentDoc())
                .ReturnsAsync(expectedString);

            // Act
            var response = await controller.GetStudentDoc();

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var result = Assert.IsAssignableFrom<string>(res.Value);
            Assert.Equal(expectedString, result);
        }
        #endregion
    }
}
