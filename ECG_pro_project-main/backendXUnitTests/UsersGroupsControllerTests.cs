using backend.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Moq;

namespace backendXUnitTests
{
    public class UsersGroupsControllerTests
    {
        #region GetGroupsInfoAsync
        [Fact]
        public async void GetGroupsInfoAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var returnUser = new User { };
            var returnUsersAndGroupsDTO = new UsersAndGroupsDTO { };
            Dictionary<string, List<UserDTO>> expectedValue = new();
            expectedValue["teacher"] = new List<UserDTO> { new UserDTO { } };
            returnUsersAndGroupsDTO.UsersAndGroups = expectedValue;

            // Arrange
            var usersGroupService = new Mock<IUserGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new UsersGroupsController(usersGroupService.Object) { ControllerContext = { HttpContext = httpContext } };

            usersGroupService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            usersGroupService.Setup(x => x.GetGroupInfoFromUser(It.IsAny<int>()))
                .ReturnsAsync(returnUsersAndGroupsDTO);

            // Act
            var response = await controller.GetGroupsInfo();

            // Arrange
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);

            var actualValue = Assert.IsAssignableFrom<UsersAndGroupsDTO>(res.Value);
            Assert.Equal(expectedValue["teacher"], actualValue.UsersAndGroups["teacher"]);
        }

        [Fact]
        public async void GetGroupsInfoAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var usersGroupService = new Mock<IUserGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new UsersGroupsController(usersGroupService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.GetGroupsInfo();

            // Arrange
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion
    }
}
