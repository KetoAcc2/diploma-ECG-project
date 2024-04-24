using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace backend.Controllers;
[Authorize]
[ApiController]
[Route(ApiRoutes.Task.MainRoute)]
public class TaskController : ControllerBase
{
    private readonly ITaskService _service;
    public TaskController(ITaskService service)
    {
        _service = service;
    }

    [HttpGet(ApiRoutes.Task.Tasks)]
    public async Task<IActionResult> GetTasks()
    {
        return Ok(await _service.GetTasks());
    }

    [HttpGet(ApiRoutes.Task.TasksByUser)]
    public async Task<IActionResult> GetTasksByUser()
    {
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? user = await _service.GetUserByAccessToken(accessToken);
        if (user == null)
        {
            return StatusCode(401);
        }

        return Ok(await _service.GetTasksByUser(user.Id));
    }

    [HttpPost(ApiRoutes.Task.TasksByGroup)]
    public async Task<IActionResult> GetTasksByGroup(TasksByGroupDTO dto)
    {
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? user = await _service.GetUserByAccessToken(accessToken);
        if (user == null)
        {
            return StatusCode(401);
        }
        if (!await _service.ExistsGroup(dto.GroupId))
        {
            return StatusCode(400);
        }
        if (!await _service.IsGroupOwner(user.Id, dto.GroupId))
        {
            return StatusCode(401);
        }
        return Ok(await _service.GetTasksByGroup(dto.GroupId));
    }

    [HttpPost(ApiRoutes.Task.FinishedTasksByGroup)]
    public async Task<IActionResult> GetFinishedTasksByGroup(TasksByGroupDTO dto)
    {
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? user = await _service.GetUserByAccessToken(accessToken);
        if (user == null)
        {
            return StatusCode(401);
        }
        if (!await _service.ExistsGroup(dto.GroupId))
        {
            return StatusCode(400);
        }
        return Ok(await _service.GetFinishedTasksByGroup(dto.GroupId, user.Id));
    }

    [HttpPost(ApiRoutes.Task.GetFinishedTasksByGroup)]
    public async Task<IActionResult> GetFinishedTasksByGroup(TasksByGroupDTO dto, int userId)
    {
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? user = await _service.GetUserByAccessToken(accessToken);
        if (user == null)
        {
            return StatusCode(401);
        }
        if (!await _service.ExistsGroup(dto.GroupId))
        {
            return StatusCode(400);
        }
        return Ok(await _service.GetFinishedTasksByGroup(dto.GroupId, userId));
    }

    [HttpGet(ApiRoutes.Task.GetTask)]
    public async Task<IActionResult> GetTask(int taskId)
    {
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? user = await _service.GetUserByAccessToken(accessToken);
        if (user == null)
        {
            return StatusCode(401);
        }
        return Ok(await _service.GetTask(taskId));
    }

    //TODO: removed, fix frontend
    // [HttpGet("TaskContent/{taskId}")]

    //TODO: removed, fix frontend
    // [HttpGet("Questions")]

    //TODO: removed, fix frontend
    // [HttpGet("Answers")]

    //TODO: removed, fix frontend
    // [HttpGet("AnswersQuestions")]

    //TODO: removed, fix frontend
    // [HttpGet("QuestionsOfType/{questionTypeId}")]


    [HttpGet(ApiRoutes.Task.FinishedTasks)]
    public async Task<IActionResult> GetFinishedTasks()
    {
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? user = await _service.GetUserByAccessToken(accessToken);
        if (user == null)
        {
            return StatusCode(401);
        }
        return Ok(await _service.GetFinishedTasksByUser(user.Id));
    }

    [HttpGet(ApiRoutes.Task.GetTaskHistory)]
    public async Task<IActionResult> GetTaskHistory(int taskId)
    {
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? user = await _service.GetUserByAccessToken(accessToken);
        if (user == null)
        {
            return StatusCode(401);
        }
        try
        {
            return Ok(await _service.GetTaskHistory(user.Id, taskId));
        }
        catch (Exception)
        {
             
            return StatusCode(404);
        }
    }

    [HttpGet(ApiRoutes.Task.GetUserTaskHistory)]
    [Authorize(Roles = UserRoleType.TEACHER)]
    public async Task<IActionResult> GetUserTaskHistory(int taskId, int userId)
    {
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? user = await _service.GetUserByAccessToken(accessToken);
        if (user == null)
        {
            return StatusCode(401);
        }
        if (user.Role.ToLower() != "teacher")
        {
            return StatusCode(403);
        }
        try
        {
            return Ok(await _service.GetTaskHistory(userId, taskId));
        }
        catch (Exception)
        {
             
            return StatusCode(404);
        }
    }

    [HttpGet(ApiRoutes.Task.OverallScore)]
    public async Task<IActionResult> GetOverallScore()
    {
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? user = await _service.GetUserByAccessToken(accessToken);
        if (user == null)
        {
            return StatusCode(401);
        }

        return Ok(await _service.GetOverAllScoreForEachGroup(user.Id));
    }

    [HttpGet(ApiRoutes.Task.QuestionType)]
    public async Task<IActionResult> GetQuestionTypes()
    {
        return Ok(await _service.GetQuestionTypes());
    }

    //is this needed still?
    [HttpGet(ApiRoutes.Task.TaskQuestions)]
    public async Task<IActionResult> GetTaskQuestions()
    {
        return Ok(await _service.GetTaskQuestion());
    }

    [HttpPost(ApiRoutes.Task.AssignTasks)]
    public async Task<IActionResult> AssignTasks(AssignTasksDTO data)
    {
        if (data.Groups is null || data.Tasks is null)
        {
            return StatusCode(400);
        }

        foreach (var item in data.Groups)
        {
            if (await _service.GetGroup(item) == null)
            {
                return StatusCode(404);
            }
        }
        foreach (var item in data.Tasks)
        {
            if (await _service.GetQuestionType(item) == null)
            {
                return StatusCode(404);
            }
        }
        bool success = await _service.AssignTasks(data);
        if (!success)
        {
            return StatusCode(400);
        }
        return StatusCode(200);
    }

    [HttpPost(ApiRoutes.Task.SubmitTask)]
    public async Task<IActionResult> SubmitTask(TaskSubmissionDTO taskSubmissionDTO)
    {
         
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? user = await _service.GetUserByAccessToken(accessToken);
        if (user == null)
        {
            return StatusCode(401);
        }
        if (await _service.HandleTaskSubmission(taskSubmissionDTO, user.Id))
        {
            return StatusCode(200);
        }
        return StatusCode(400);
    }

    [HttpGet(ApiRoutes.Task.UserStats)]
    public async Task<IActionResult> UserStats()
    {
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? user = await _service.GetUserByAccessToken(accessToken);
        if (user == null)
        {
            return StatusCode(401);
        }
        return Ok(await _service.GetUserStats(user.Id));
    }

    [HttpDelete(ApiRoutes.Task.RemoveTask)]
    public async Task<IActionResult> RemoveTask(RemoveTaskDTO dto)
    {
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? user = await _service.GetUserByAccessToken(accessToken);
        if (user == null)
        {
            return StatusCode(401);
        }
        if (!await _service.HasRemoveTaskPrivilege(user.Id, dto.GroupId))
        {
            return StatusCode(401);
        }
        if (!await _service.TaskIsAssignedToGroup(dto.TaskId, dto.GroupId))
        {
            return StatusCode(403);
        }
        await _service.RemoveTask(dto.TaskId, dto.GroupId);
        return StatusCode(200);
    }

    [HttpGet("Patients")]
    public async Task<IActionResult> GetPatients()
    {

        return Ok(await _service.GetPatients());
    }

    [HttpGet("Patient")]
    public async Task<IActionResult> GetPatient(int patientId)
    {

        return Ok(await _service.GetPatient(patientId));
    }

    [HttpGet(ApiRoutes.Task.GetTaskScore)]
    public async Task<IActionResult> GetTaskScore(int taskGroupId)
    {

        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? user = await _service.GetUserByAccessToken(accessToken);
        if (user == null)
        {
            return StatusCode(401);
        }
        if (!await _service.ExistsTaskGroup(taskGroupId))
        {
            return StatusCode(404);
        }
        return Ok(await _service.ScorePerTask(taskGroupId));
    }

    [HttpGet(ApiRoutes.Task.Pics)]
    public async Task<IActionResult> GetPic(int taskId)
    {
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? user = await _service.GetUserByAccessToken(accessToken);
        if (user == null)
        {
            return StatusCode(401);
        }
        try
        {
            var result = Ok(await _service.GetDiagramForTask(taskId, user.Id));
            return result;
        }
        catch (System.Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet(ApiRoutes.Task.GetECGMs)]
    public async Task<IActionResult> GetECGMs(int taskId)
    {
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? user = await _service.GetUserByAccessToken(accessToken);
        if (user == null)
        {
            return StatusCode(401);
        }
        try
        {
            var result = Ok(await _service.GetDiagramMsForTask(taskId, user.Id));
            return result;
        }
        catch (System.Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet(ApiRoutes.Task.GetTaskSummary)]
    public async Task<IActionResult> GetTaskSummary(int taskId)
    {
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? user = await _service.GetUserByAccessToken(accessToken);
        if (user == null)
        {
            return StatusCode(401);
        }
        return Ok(await _service.GetTaskSummaryForTask(taskId, user.Id));
    }
}
