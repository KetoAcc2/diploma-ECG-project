using backend.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Moq;

namespace backendXUnitTests
{
    public class GroupsControllerTests
    {
        #region GetGroupsAsync
        [Fact]
        public async void GetGroupsAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedValue = new List<Group> { new Group { } };

            // Arrange
            var groupService = new Mock<IGroupService>();
            groupService.Setup(x => x.GetGroupsAsync())
                .ReturnsAsync(expectedValue);

            // Act
            var controller = new GroupsController(groupService.Object);
            var response = await controller.GetGroups();

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualList = Assert.IsAssignableFrom<List<Group>>(res.Value);
            Assert.Single(actualList);
            Assert.Equal(expectedValue[0], actualList[0]);
        }
        #endregion

        #region GetGroupCodeAsync
        [Fact]
        public async void GetGroupCodeAsync_IntededInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedGroupCode = "testcode";
            var returnValue = new Group { GroupCode = expectedGroupCode };

            // Arrange
            var groupService = new Mock<IGroupService>();
            groupService.Setup(x => x.GetGroupAsync(It.IsAny<int>()))
                .ReturnsAsync(returnValue);

            // Act
            var controller = new GroupsController(groupService.Object);
            var response = await controller.GetGroupCode(0);

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualValue = Assert.IsAssignableFrom<GroupCodeDTO>(res.Value);
            Assert.Equal(expectedGroupCode, actualValue.GroupCode);
        }

        [Fact]
        public async void GetGroupCodeAsync_ProvidingWrongGroup_Returns400()
        {
            var expectedStatusCode = 400;

            // Arrange
            var groupService = new Mock<IGroupService>();

            // Act
            var controller = new GroupsController(groupService.Object);
            var response = await controller.GetGroupCode(0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region GetJoinedGroupsAsync
        [Fact]
        public async void GetJoinedGroupsAsync_IntededInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedGroupCode = "testcode";
            var returnUser = new User { };
            var returnGroups = new List<Group> { new Group { GroupCode = expectedGroupCode } };

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            groupService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            groupService.Setup(x => x.GetJoinedGroupsAsync(It.IsAny<int>()))
                .ReturnsAsync(returnGroups);

            // Act
            var response = await controller.GetJoinedGroups();

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualValue = Assert.IsAssignableFrom<List<Group>>(res.Value);
            Assert.Single(actualValue);
            Assert.Equal(expectedGroupCode, actualValue[0].GroupCode);
        }

        [Fact]
        public async void GetJoinedGroupsAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.GetJoinedGroups();

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region GetUserJoinedGroupsByIdAsync
        [Fact]
        public async void GetUserJoinedGroupsByIdAsync_IntededInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedGroupCode = "testcode";
            var returnUser1 = new User { };
            var returnUser2 = new User { };
            var returnGroups = new List<Group> { new Group { GroupCode = expectedGroupCode } };

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            groupService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser1);

            groupService.Setup(x => x.GetJoinedGroupsAsync(It.IsAny<int>()))
                .ReturnsAsync(returnGroups);

            groupService.Setup(x => x.GetUserById(It.IsAny<int>()))
                .ReturnsAsync(returnUser2);

            // Act
            var response = await controller.GetUserJoinedGroupsById(0);

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualValue = Assert.IsAssignableFrom<List<Group>>(res.Value);
            Assert.Single(actualValue);
            Assert.Equal(expectedGroupCode, actualValue[0].GroupCode);
        }

        [Fact]
        public async void GetUserJoinedGroupsByIdAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.GetUserJoinedGroupsById(0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void GetUserJoinedGroupsByIdAsync_ProvidingWrongUserId_Returns404()
        {
            var expectedStatusCode = 404;
            var returnUser1 = new User { };

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            groupService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser1);

            // Act
            var response = await controller.GetUserJoinedGroupsById(0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region GetCreatedGroupsAsync
        [Fact]
        public async void GetCreatedGroupsAsync_IntededInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedGroupCode = "testcode";
            var returnUser = new User { };
            var returnGroups = new List<Group> { new Group { GroupCode = expectedGroupCode } };

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            groupService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            groupService.Setup(x => x.GetCreatedGroupsAsync(It.IsAny<int>()))
                .ReturnsAsync(returnGroups);

            // Act
            var response = await controller.GetCreatedGroups();

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualValue = Assert.IsAssignableFrom<List<Group>>(res.Value);
            Assert.Single(actualValue);
            Assert.Equal(expectedGroupCode, actualValue[0].GroupCode);
        }

        [Fact]
        public async void GetCreatedGroupsAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.GetCreatedGroups();

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region CreateNewGroupAsync
        [Fact]
        public async void CreateNewGroupAsync_IntededInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var returnUser = new User { Role = "Teacher" };
            var returnGroup =  new Group { };

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            groupService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            groupService.Setup(x => x.CreateGroup(It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(returnGroup);

            groupService.Setup(x => x.JoinGroup(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var response = await controller.CreateNewGroup(new CreateGroupDTO { GroupName = "testname" });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void CreateNewGroupAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.CreateNewGroup(new CreateGroupDTO { GroupName = "testname" });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void CreateNewGroupAsync_ProvidingNotTeacher_Returns403()
        {
            var expectedStatusCode = 403;
            var returnUser = new User { Role = "Student" };

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            groupService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            // Act
            var response = await controller.CreateNewGroup(new CreateGroupDTO {});

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void CreateNewGroupAsync_ProvidingWrongData_Returns400()
        {
            var expectedStatusCode = 400;

            // Arrange
            var groupService = new Mock<IGroupService>();

            var controller = new GroupsController(groupService.Object);

            // Act
            var response = await controller.CreateNewGroup(null);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void CreateNewGroupAsync_ProvidingWrongUserIdInJoinGroup_Returns400()
        {
            var expectedStatusCode = 400;
            var returnUser = new User { Role = "Teacher" };
            var returnGroup = new Group { };

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            groupService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            groupService.Setup(x => x.CreateGroup(It.IsAny<string>(), It.IsAny<User>()))
                .ReturnsAsync(returnGroup);

            // Act
            var response = await controller.CreateNewGroup(new CreateGroupDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region JoinGroupAsync
        [Fact]
        public async void JoinGroupAsync_IntededInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var returnUser = new User { };
            var expectedGroupCode = "testcode";
            var returnGroup = new Group { GroupCode = expectedGroupCode };

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            groupService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            groupService.Setup(x => x.GetGroupByGroupCode(It.IsAny<string>()))
                .ReturnsAsync(returnGroup);

            groupService.Setup(x => x.ExistsUserInGroup(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            groupService.Setup(x => x.JoinGroup(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var response = await controller.JoinGroup(new GroupCodeDTO { GroupCode = expectedGroupCode });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void JoinGroupAsync_ProvidingWrongData_Returns400()
        {
            var expectedStatusCode = 400;

            // Arrange
            var groupService = new Mock<IGroupService>();

            var controller = new GroupsController(groupService.Object);

            // Act
            var response = await controller.JoinGroup(new GroupCodeDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void JoinGroupAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;
            var returnUser = new User { };
            var expectedGroupCode = "testcode";

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.JoinGroup(new GroupCodeDTO { GroupCode = expectedGroupCode });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void JoinGroupAsync_ProvidingWrongGroupCode_Returns400()
        {
            var expectedStatusCode = 400;
            var returnUser = new User { };
            var expectedGroupCode = "testcode";

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            groupService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            // Act
            var response = await controller.JoinGroup(new GroupCodeDTO { GroupCode = expectedGroupCode });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void JoinGroupAsync_UserDoesExistInGroup_Returns304()
        {
            var expectedStatusCode = 304;
            var returnUser = new User { };
            var expectedGroupCode = "testcode";
            var returnGroup = new Group { GroupCode = expectedGroupCode };

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            groupService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            groupService.Setup(x => x.GetGroupByGroupCode(It.IsAny<string>()))
                .ReturnsAsync(returnGroup);

            groupService.Setup(x => x.ExistsUserInGroup(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var response = await controller.JoinGroup(new GroupCodeDTO { GroupCode = expectedGroupCode });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void JoinGroupAsync_UnsuccessfulJoinGroup_Returns400()
        {
            var expectedStatusCode = 400;
            var returnUser = new User { };
            var expectedGroupCode = "testcode";
            var returnGroup = new Group { GroupCode = expectedGroupCode };

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            groupService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            groupService.Setup(x => x.GetGroupByGroupCode(It.IsAny<string>()))
                .ReturnsAsync(returnGroup);

            groupService.Setup(x => x.ExistsUserInGroup(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            groupService.Setup(x => x.JoinGroup(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var response = await controller.JoinGroup(new GroupCodeDTO { GroupCode = expectedGroupCode });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region DeleteGroupAsync
        [Fact]
        public async void DeleteGroupAsync_IntededInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var returnUser = new User { Role = "Teacher" };
            var returnGroup = new Group { };

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            groupService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            groupService.Setup(x => x.GetGroupAsync(It.IsAny<int>()))
                .ReturnsAsync(returnGroup);

            groupService.Setup(x => x.DeleteGroup(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var response = await controller.DeleteGroup(0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void DeleteGroupAsync_ProvidingWrongGroupId_Returns400()
        {
            var expectedStatusCode = 400;

            // Arrange
            var groupService = new Mock<IGroupService>();

            var controller = new GroupsController(groupService.Object);
            // Act
            var response = await controller.DeleteGroup(-1);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void DeleteGroupAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.DeleteGroup(0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void DeleteGroupAsync_ProvidingNotTeacher_Returns403()
        {
            var expectedStatusCode = 403;
            var returnUser = new User { Role = "Student" };

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            groupService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            // Act
            var response = await controller.DeleteGroup(0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void DeleteGroupAsync_ProvidingWrongGroupIdToGetGroup_Returns400()
        {
            var expectedStatusCode = 400;
            var returnUser = new User { Role = "Teacher" };

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            groupService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            // Act
            var response = await controller.DeleteGroup(0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void DeleteGroupAsync_ProvidingNotGroupOwner_Returns401()
        {
            var expectedStatusCode = 401;
            var returnUser = new User { Id = 1, Role = "Teacher" };
            var returnGroup = new Group { GroupOwner = 0 };

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            groupService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            groupService.Setup(x => x.GetGroupAsync(It.IsAny<int>()))
                .ReturnsAsync(returnGroup);

            // Act
            var response = await controller.DeleteGroup(1);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void DeleteGroupAsync_UnsuccessfulDeleteGroup_Returns400()
        {
            var expectedStatusCode = 400;
            var returnUser = new User { Role = "Teacher" };
            var returnGroup = new Group { };

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            groupService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            groupService.Setup(x => x.GetGroupAsync(It.IsAny<int>()))
                .ReturnsAsync(returnGroup);

            groupService.Setup(x => x.DeleteGroup(It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var response = await controller.DeleteGroup(0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region RemoveUserFromGroupAsync
        [Fact]
        public async void RemoveUserFromGroupAsync_IntededInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var returnUser = new User { Id = 0, Role = "Teacher" };

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            groupService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            groupService.Setup(x => x.DeleteUserFromGroup(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var response = await controller.RemoveUserFromGroup(new RemoveUserFromGroupDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void RemoveUserFromGroupAsync_UnsuccessfulDeleteUserFromGroup_Returns400()
        {
            var expectedStatusCode = 400;
            var returnUser = new User { Id = 0, Role = "Teacher" };

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            groupService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            groupService.Setup(x => x.DeleteUserFromGroup(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var response = await controller.RemoveUserFromGroup(new RemoveUserFromGroupDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region RegenGroupCodeAsync
        [Fact]
        public async void RegenGroupCodeAsync_IntededInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var returnUser = new User { Id = 0, Role = "Teacher" };
            var returnGroup = new Group { GroupOwner = 0 };

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            groupService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            groupService.Setup(x => x.GetGroupAsync(It.IsAny<int>()))
                .ReturnsAsync(returnGroup);

            groupService.Setup(x => x.RegenGroupCode(It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var response = await controller.RegenGroupCode(new GroupCodeRegenDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void RegenGroupCodeAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.RegenGroupCode(new GroupCodeRegenDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void RegenGroupCodeAsync_ProvidingWrongDataGroupId_Returns400()
        {
            var expectedStatusCode = 400;
            var returnUser = new User { Role = UserRoleType.TEACHER };

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            groupService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            // Act
            var response = await controller.RegenGroupCode(new GroupCodeRegenDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void RegenGroupCodeAsync_ProvidingNotGroupOwner_Returns401()
        {
            var expectedStatusCode = 401;
            var returnUser = new User { Id = 1, Role = "Teacher" };
            var returnGroup = new Group { GroupOwner = 0 };

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            groupService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            groupService.Setup(x => x.GetGroupAsync(It.IsAny<int>()))
                .ReturnsAsync(returnGroup);

            // Act
            var response = await controller.RegenGroupCode(new GroupCodeRegenDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void RegenGroupCodeAsync_UnsuccessfulRegenGroupCode_Returns400()
        {
            var expectedStatusCode = 400;
            var returnUser = new User { Id = 0, Role = "Teacher" };
            var returnGroup = new Group { GroupOwner = 0 };

            // Arrange
            var groupService = new Mock<IGroupService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new GroupsController(groupService.Object) { ControllerContext = { HttpContext = httpContext } };

            groupService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            groupService.Setup(x => x.GetGroupAsync(It.IsAny<int>()))
                .ReturnsAsync(returnGroup);

            groupService.Setup(x => x.RegenGroupCode(It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var response = await controller.RegenGroupCode(new GroupCodeRegenDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion
    }
}
