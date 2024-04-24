using backend.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Moq;

namespace backendXUnitTests
{
    public class TasksControllerTests
    {
        #region GetTasksAsync
        [Fact]
        public async void GetTasksAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedValue = new List<TaskDTO> { new TaskDTO { } };

            // Arrange
            var taskService = new Mock<ITaskService>();
            taskService.Setup(x => x.GetTasks())
                .ReturnsAsync(expectedValue);

            // Act
            var controller = new TaskController(taskService.Object);
            var response = await controller.GetTasks();

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualList = Assert.IsAssignableFrom<List<TaskDTO>>(res.Value);
            Assert.Single(actualList);
            Assert.Equal(expectedValue[0], actualList[0]);
        }
        #endregion

        #region GetTasksByUserAsync
        [Fact]
        public async void GetTasksByUserAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedValue = new List<TaskDTO> { new TaskDTO { } };
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.GetTasksByUser(It.IsAny<int>()))
                .ReturnsAsync(expectedValue);

            // Act
            var response = await controller.GetTasksByUser();

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualList = Assert.IsAssignableFrom<List<TaskDTO>>(res.Value);
            Assert.Single(actualList);
            Assert.Equal(expectedValue[0], actualList[0]);
        }

        [Fact]
        public async void GetTasksByUserAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.GetTasksByUser();

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region GetTasksByGroupAsync
        [Fact]
        public async void GetTasksByGroupAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedValue = new List<TaskDTO?> { new TaskDTO { } };
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.ExistsGroup(It.IsAny<int>()))
                .ReturnsAsync(true);

            taskService.Setup(x => x.IsGroupOwner(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            taskService.Setup(x => x.GetTasksByGroup(It.IsAny<int>()))
                .ReturnsAsync(expectedValue);

            // Act
            var response = await controller.GetTasksByGroup(new TasksByGroupDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualList = Assert.IsAssignableFrom<List<TaskDTO>>(res.Value);
            Assert.Single(actualList);
            Assert.Equal(expectedValue[0], actualList[0]);
        }

        [Fact]
        public async void GetTasksByGroupAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.GetTasksByGroup(new TasksByGroupDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void GetTasksByGroupAsync_UnsuccessfulExistsGroup_Returns400()
        {
            var expectedStatusCode = 400;
            var expectedValue = new List<TaskDTO?> { new TaskDTO { } };
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.ExistsGroup(It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var response = await controller.GetTasksByGroup(new TasksByGroupDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void GetTasksByGroupAsync_UnsuccessfulIsGroupOwner_Returns401()
        {
            var expectedStatusCode = 401;
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.ExistsGroup(It.IsAny<int>()))
                .ReturnsAsync(true);

            taskService.Setup(x => x.IsGroupOwner(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var response = await controller.GetTasksByGroup(new TasksByGroupDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region GetFinishedTasksByGroupAsync
        [Fact]
        public async void GetFinishedTasksByGroupAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedValue = new List<FinishedTasksByGroup> { new FinishedTasksByGroup { } };
            var returnValue = new FinishedTasksByGroupDTO { FinishedTasks = expectedValue };
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.ExistsGroup(It.IsAny<int>()))
                .ReturnsAsync(true);

            taskService.Setup(x => x.GetFinishedTasksByGroup(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(returnValue);

            // Act
            var response = await controller.GetFinishedTasksByGroup(new TasksByGroupDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualValue = Assert.IsAssignableFrom<FinishedTasksByGroupDTO>(res.Value);
            Assert.Equal(expectedValue[0], actualValue.FinishedTasks[0]);
        }

        [Fact]
        public async void GetFinishedTasksByGroupAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.GetFinishedTasksByGroup(new TasksByGroupDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void GetFinishedTasksByGroupAsync_UnsuccessfulExistsGroup_Returns400()
        {
            var expectedStatusCode = 400;
            var expectedValue = new List<FinishedTasksByGroup> { new FinishedTasksByGroup { } };
            var returnValue = new FinishedTasksByGroupDTO { FinishedTasks = expectedValue };
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.ExistsGroup(It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var response = await controller.GetFinishedTasksByGroup(new TasksByGroupDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region GetFinishedTasksByGroupAsync with two parameters
        [Fact]
        public async void GetFinishedTasksByGroupAsync2_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedValue = new List<FinishedTasksByGroup> { new FinishedTasksByGroup { } };
            var returnValue = new FinishedTasksByGroupDTO { FinishedTasks = expectedValue };
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.ExistsGroup(It.IsAny<int>()))
                .ReturnsAsync(true);

            taskService.Setup(x => x.GetFinishedTasksByGroup(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(returnValue);

            // Act
            var response = await controller.GetFinishedTasksByGroup(new TasksByGroupDTO { }, 0);

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualValue = Assert.IsAssignableFrom<FinishedTasksByGroupDTO>(res.Value);
            Assert.Equal(expectedValue[0], actualValue.FinishedTasks[0]);
        }

        [Fact]
        public async void GetFinishedTasksByGroupAsync2_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.GetFinishedTasksByGroup(new TasksByGroupDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void GetFinishedTasksByGroupAsync2_UnsuccessfulExistsGroup_Returns400()
        {
            var expectedStatusCode = 400;
            var expectedValue = new List<FinishedTasksByGroup> { new FinishedTasksByGroup { } };
            var returnValue = new FinishedTasksByGroupDTO { FinishedTasks = expectedValue };
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.ExistsGroup(It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var response = await controller.GetFinishedTasksByGroup(new TasksByGroupDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region GetTaskAsync
        [Fact]
        public async void GetTaskAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedValue = new Task { TaskId = 0 };
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.GetTask(It.IsAny<int>()))
                .ReturnsAsync(expectedValue);

            // Act
            var response = await controller.GetTask(0);

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualValue = Assert.IsAssignableFrom<Task>(res.Value);
            Assert.Equal(expectedValue.TaskId, actualValue.TaskId);
        }

        [Fact]
        public async void GetTaskAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.GetTask(0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region GetFinishedTasksAsync
        [Fact]
        public async void GetFinishedTasksAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedValue = new List<TaskHistoryDTO> { new TaskHistoryDTO { } };
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.GetFinishedTasksByUser(It.IsAny<int>()))
                .ReturnsAsync(expectedValue);

            // Act
            var response = await controller.GetFinishedTasks();

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualValue = Assert.IsAssignableFrom<List<TaskHistoryDTO>>(res.Value);
            Assert.Single(actualValue);
            Assert.Equal(expectedValue[0], actualValue[0]);
        }

        [Fact]
        public async void GetFinishedTasksAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.GetFinishedTasks();

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region GetTaskHistoryAsync
        [Fact]
        public async void GetTaskHistoryAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedValue = new TaskHistoryResponseDTO { };
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.GetTaskHistory(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedValue);

            // Act
            var response = await controller.GetTaskHistory(0);

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualValue = Assert.IsAssignableFrom<TaskHistoryResponseDTO>(res.Value);
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public async void GetTaskHistoryAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.GetTaskHistory(0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void GetTaskHistoryAsync_GetTaskHistoryThrowsException_Returns404()
        {
            var expectedStatusCode = 404;
            var expectedValue = new TaskHistoryResponseDTO { };
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.GetTaskHistory(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            // Act
            var response = await controller.GetTaskHistory(0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region GetUserTaskHistoryAsync
        [Fact]
        public async void GetUserTaskHistoryAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedValue = new TaskHistoryResponseDTO { };
            var returnUser = new User { Role = UserRoleType.TEACHER };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.GetTaskHistory(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedValue);

            // Act
            var response = await controller.GetUserTaskHistory(0, 0);

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualValue = Assert.IsAssignableFrom<TaskHistoryResponseDTO>(res.Value);
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public async void GetUserTaskHistoryAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.GetUserTaskHistory(0, 0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void GetUserTaskHistoryAsync_ProvidingNotTeacher_Returns403()
        {
            var expectedStatusCode = 403;
            var returnUser = new User { Role = UserRoleType.STUDENT };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);
            // Act
            var response = await controller.GetUserTaskHistory(0, 0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void GetUserTaskHistoryAsync_GetTaskHistoryThrowsException_Returns404()
        {
            var expectedStatusCode = 404;
            var expectedValue = new TaskHistoryResponseDTO { };
            var returnUser = new User { Role = UserRoleType.TEACHER };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.GetTaskHistory(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            // Act
            var response = await controller.GetUserTaskHistory(0, 0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region GetOverallScoreAsync
        [Fact]
        public async void GetOverallScoreAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedValue = new OverallScorePerGroupDTO { };
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.GetOverAllScoreForEachGroup(It.IsAny<int>()))
                .ReturnsAsync(expectedValue);

            // Act
            var response = await controller.GetOverallScore();

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualValue = Assert.IsAssignableFrom<OverallScorePerGroupDTO>(res.Value);
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public async void GetOverallScoreAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.GetOverallScore();

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region GetQuestionTypesAsync
        [Fact]
        public async void GetQuestionTypesAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedValue = new List<QuestionTypeDTO> { new QuestionTypeDTO { } };

            // Arrange
            var taskService = new Mock<ITaskService>();

            taskService.Setup(x => x.GetQuestionTypes())
                .ReturnsAsync(expectedValue);

            // Act
            var controller = new TaskController(taskService.Object);
            var response = await controller.GetQuestionTypes();

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualValue = Assert.IsAssignableFrom<List<QuestionTypeDTO>>(res.Value);
            Assert.Single(actualValue);
            Assert.Equal(expectedValue[0], actualValue[0]);
        }
        #endregion

        #region GetTaskQuestionsAsync
        [Fact]
        public async void GetTaskQuestionsAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedValue = new List<Task_Question> { new Task_Question { } };

            // Arrange
            var taskService = new Mock<ITaskService>();

            taskService.Setup(x => x.GetTaskQuestion())
                .ReturnsAsync(expectedValue);

            // Act
            var controller = new TaskController(taskService.Object);
            var response = await controller.GetTaskQuestions();

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualValue = Assert.IsAssignableFrom<List<Task_Question>>(res.Value);
            Assert.Single(actualValue);
            Assert.Equal(expectedValue[0], actualValue[0]);
        }
        #endregion

        #region AssignTasksAsync
        [Fact]
        public async void AssignTasksAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var returnGroup = new Group { };
            var returnQuestionType = new Question_Type { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            taskService.Setup(x => x.GetGroup(It.IsAny<int>()))
                .ReturnsAsync(returnGroup);

            taskService.Setup(x => x.GetQuestionType(It.IsAny<int>()))
                .ReturnsAsync(returnQuestionType);

            taskService.Setup(x => x.AssignTasks(It.IsAny<AssignTasksDTO>()))
                .ReturnsAsync(true);

            // Act
            var controller = new TaskController(taskService.Object);
            var response = await controller.AssignTasks(new AssignTasksDTO
            {
                Groups = new int[] { 0 },
                Tasks = new int[] { 0 }
            });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void AssignTasksAsync_ProvidingWrongData_Returns400()
        {
            var expectedStatusCode = 400;

            // Arrange
            var taskService = new Mock<ITaskService>();

            // Act
            var controller = new TaskController(taskService.Object);
            var response = await controller.AssignTasks(new AssignTasksDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void AssignTasksAsync_ProvidingWrongGroups_Returns404()
        {
            var expectedStatusCode = 404;

            // Arrange
            var taskService = new Mock<ITaskService>();

            // Act
            var controller = new TaskController(taskService.Object);
            var response = await controller.AssignTasks(new AssignTasksDTO
            {
                Groups = new int[] { 0 },
                Tasks = new int[] { 0 }
            });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void AssignTasksAsync_ProvidingWrongTasks_Returns404()
        {
            var expectedStatusCode = 404;
            var returnGroup = new Group { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            taskService.Setup(x => x.GetGroup(It.IsAny<int>()))
                .ReturnsAsync(returnGroup);

            // Act
            var controller = new TaskController(taskService.Object);
            var response = await controller.AssignTasks(new AssignTasksDTO
            {
                Groups = new int[] { 0 },
                Tasks = new int[] { 0 }
            });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void AssignTasksAsync_UnsuccessfulAssignTasks_Returns400()
        {
            var expectedStatusCode = 400;
            var returnGroup = new Group { };
            var returnQuestionType = new Question_Type { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            taskService.Setup(x => x.GetGroup(It.IsAny<int>()))
                .ReturnsAsync(returnGroup);

            taskService.Setup(x => x.GetQuestionType(It.IsAny<int>()))
                .ReturnsAsync(returnQuestionType);

            taskService.Setup(x => x.AssignTasks(It.IsAny<AssignTasksDTO>()))
                .ReturnsAsync(false);

            // Act
            var controller = new TaskController(taskService.Object);
            var response = await controller.AssignTasks(new AssignTasksDTO
            {
                Groups = new int[] { 0 },
                Tasks = new int[] { 0 }
            });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region SubmitTaskAsync
        [Fact]
        public async void SubmitTaskAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedValue = new TaskHistoryResponseDTO { };
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.HandleTaskSubmission(It.IsAny<TaskSubmissionDTO>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var response = await controller.SubmitTask(new TaskSubmissionDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void SubmitTaskAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.SubmitTask(new TaskSubmissionDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void SubmitTaskAsync_UnsuccessfulHandleTaskSubmission_Returns400()
        {
            var expectedStatusCode = 400;
            var expectedValue = new TaskHistoryResponseDTO { };
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.HandleTaskSubmission(It.IsAny<TaskSubmissionDTO>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var response = await controller.SubmitTask(new TaskSubmissionDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region UserStatsAsync
        [Fact]
        public async void UserStatsAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            double expectedValue = 4.5;
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.GetUserStats(It.IsAny<int>()))
                .ReturnsAsync(expectedValue);

            // Act
            var response = await controller.UserStats();

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualValue = Assert.IsAssignableFrom<double>(res.Value);
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public async void UserStatsAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.UserStats();

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region RemoveTaskAsync
        [Fact]
        public async void RemoveTaskAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.HasRemoveTaskPrivilege(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            taskService.Setup(x => x.TaskIsAssignedToGroup(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            taskService.Setup(x => x.RemoveTask(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var response = await controller.RemoveTask(new RemoveTaskDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void RemoveTaskAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.RemoveTask(new RemoveTaskDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void RemoveTaskAsync_UnsuccessfulHasRemoveTaskPrivilige_Returns401()
        {
            var expectedStatusCode = 401;
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.HasRemoveTaskPrivilege(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var response = await controller.RemoveTask(new RemoveTaskDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void RemoveTaskAsync_UnsuccessfulTaskIsAssignedToGroup_Returns403()
        {
            var expectedStatusCode = 403;
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
               .ReturnsAsync(returnUser);

            taskService.Setup(x => x.HasRemoveTaskPrivilege(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            taskService.Setup(x => x.TaskIsAssignedToGroup(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var response = await controller.RemoveTask(new RemoveTaskDTO { });

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region GetPatientsAsync
        [Fact]
        public async void GetPatientsAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedValue = new List<Patient> { new Patient { } };

            // Arrange
            var taskService = new Mock<ITaskService>();

            taskService.Setup(x => x.GetPatients())
                .ReturnsAsync(expectedValue);

            // Act
            var controller = new TaskController(taskService.Object);
            var response = await controller.GetPatients();

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualValue = Assert.IsAssignableFrom<List<Patient>>(res.Value);
            Assert.Single(actualValue);
            Assert.Equal(expectedValue[0], actualValue[0]);
        }
        #endregion

        #region GetPatientAsync
        [Fact]
        public async void GetPatientAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedValue = new Patient { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            taskService.Setup(x => x.GetPatient(It.IsAny<int>()))
                .ReturnsAsync(expectedValue);

            // Act
            var controller = new TaskController(taskService.Object);
            var response = await controller.GetPatient(0);

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualValue = Assert.IsAssignableFrom<Patient>(res.Value);
            Assert.Equal(expectedValue, actualValue);
        }
        #endregion

        #region GetTaskScoreAsync
        [Fact]
        public async void GetTaskScoreAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            double expectedValue = 4.5;
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.ExistsTaskGroup(It.IsAny<int>()))
                .ReturnsAsync(true);

            taskService.Setup(x => x.ScorePerTask(It.IsAny<int>()))
                .ReturnsAsync(expectedValue);

            // Act
            var response = await controller.GetTaskScore(0);

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualValue = Assert.IsAssignableFrom<double>(res.Value);
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public async void GetTaskScoreAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.GetTaskScore(0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void GetTaskScoreAsync_UnsuccessfulExistsTaskGroup_Returns404()
        {
            var expectedStatusCode = 404;
            double expectedValue = 4.5;
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.ExistsTaskGroup(It.IsAny<int>()))
                .ReturnsAsync(false);

            // Act
            var response = await controller.GetTaskScore(0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region GetPicAsync
        [Fact]
        public async void GetPicAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedValue = new ECGDiagramDTO { };
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.GetDiagramForTask(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedValue);

            // Act
            var response = await controller.GetPic(0);

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualValue = Assert.IsAssignableFrom<ECGDiagramDTO>(res.Value);
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public async void GetPicAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.GetPic(0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void GetPicAsyncAsync_GetDiagramForTaskThrowsException_Returns500()
        {
            var expectedStatusCode = 500;
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.GetDiagramForTask(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            // Act
            var response = await controller.GetPic(0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region GetECGMsAsync
        [Fact]
        public async void GetECGMsAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedValue = new ECGDiagramMsDTO { };
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.GetDiagramMsForTask(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedValue);

            // Act
            var response = await controller.GetECGMs(0);

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualValue = Assert.IsAssignableFrom<ECGDiagramMsDTO>(res.Value);
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public async void GetECGMsAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.GetECGMs(0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }

        [Fact]
        public async void GetECGMsAsync_GetDiagramMsForTaskThrowsException_Returns500()
        {
            var expectedStatusCode = 500;
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.GetDiagramMsForTask(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            // Act
            var response = await controller.GetECGMs(0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion

        #region GetTaskSummaryAsync
        [Fact]
        public async void GetTaskSummaryAsync_IntendedInvocation_ReturnsOk()
        {
            var expectedStatusCode = 200;
            var expectedValue = new ECGDiagramDTO { };
            var returnUser = new User { };

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            taskService.Setup(x => x.GetUserByAccessToken(It.IsAny<string>()))
                .ReturnsAsync(returnUser);

            taskService.Setup(x => x.GetTaskSummaryForTask(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedValue);

            // Act
            var response = await controller.GetTaskSummary(0);

            // Assert
            var res = Assert.IsAssignableFrom<ObjectResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
            var actualValue = Assert.IsAssignableFrom<ECGDiagramDTO>(res.Value);
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public async void GetTaskSummaryAsync_ProvidingWrongAccessToken_Returns401()
        {
            var expectedStatusCode = 401;

            // Arrange
            var taskService = new Mock<ITaskService>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer xxxxxxxxxx";
            var controller = new TaskController(taskService.Object) { ControllerContext = { HttpContext = httpContext } };

            // Act
            var response = await controller.GetTaskSummary(0);

            // Assert
            var res = Assert.IsAssignableFrom<StatusCodeResult>(response);
            Assert.Equal(expectedStatusCode, res.StatusCode);
        }
        #endregion
    }
}
