using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
namespace backend.Controllers;

[Authorize]
[ApiController]
[Route(ApiRoutes.Group.MainRoute)]
[Produces("application/json")]
public class GroupsController : ControllerBase
{
    private readonly IGroupService _service;
    public GroupsController(IGroupService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetGroups()
    {
        return Ok(await _service.GetGroupsAsync());
    }

    [HttpGet(ApiRoutes.Group.GetGroupCode)]
    public async Task<IActionResult> GetGroupCode(int groupId)
    {
        Group? group = await _service.GetGroupAsync(groupId);
        if (group == null)
        {
            return StatusCode(400);
        }
        return Ok(new GroupCodeDTO { GroupCode = group.GroupCode });
    }

    [HttpGet(ApiRoutes.Group.JoinedGroup)]
    public async Task<IActionResult> GetJoinedGroups()
    {
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? user = await _service.GetUserByAccessToken(accessToken);
        if (user == null)
        {
            return StatusCode(401);
        }
        return Ok(await _service.GetJoinedGroupsAsync(user.Id));
    }

    [HttpGet(ApiRoutes.Group.GetUserJoinedGroupsById)]
    public async Task<IActionResult> GetUserJoinedGroupsById(int userId)
    {
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? user = await _service.GetUserByAccessToken(accessToken);
        if (user == null)
        {
            return StatusCode(401);
        }
        User? targetUser = await _service.GetUserById(userId);
        if (targetUser == null)
        {
            return StatusCode(404);
        }
        return Ok(await _service.GetJoinedGroupsAsync(userId));
    }

    [HttpGet(ApiRoutes.Group.CreatedGroup)]
    public async Task<IActionResult> GetCreatedGroups()
    {
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? user = await _service.GetUserByAccessToken(accessToken);
         
        if (user == null)
        {
            return StatusCode(401);
        }
        return Ok(await _service.GetCreatedGroupsAsync(user.Id));
    }

    [HttpPost(ApiRoutes.Group.CreateGroup)]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> CreateNewGroup(CreateGroupDTO data)
    {
        if (data == null)
        {
            return StatusCode(400);
        }
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
        Group? newGroup = await _service.CreateGroup(data.GroupName, user);
        if (newGroup == null)
        {
            return StatusCode(400);
        }
        bool success = await _service.JoinGroup(newGroup.GroupCode, user.Id);
        if (!success)
        {
            return StatusCode(400);
        }
        return StatusCode(200);
    }

    [HttpPost(ApiRoutes.Group.JoinGroup)]
    public async Task<IActionResult> JoinGroup(GroupCodeDTO data)
    {
         
        string groupCode = data.GroupCode;
        if (groupCode == null)
        {
            return StatusCode(400);
        }
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? user = await _service.GetUserByAccessToken(accessToken);
        Group? group = await _service.GetGroupByGroupCode(groupCode);
        if (user == null)
        {
            return StatusCode(401);
        }
        if (group == null)
        {
            return StatusCode(400);
        }
        if (await _service.ExistsUserInGroup(group.GroupId, user.Id))
        {
            return StatusCode(304);
        }

        if (await _service.JoinGroup(groupCode, user.Id))
        {
            return StatusCode(200);
        }
        return StatusCode(400);

    }

    [HttpDelete(ApiRoutes.Group.DeleteGroup)]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> DeleteGroup(int groupId)
    {
        if (groupId < 0)
        {
            return StatusCode(400);
        }
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
        Group? ownedGroup = await _service.GetGroupAsync(groupId);
        if (ownedGroup == null)
        {
            return StatusCode(400);
        }
        if (ownedGroup.GroupOwner != user.Id)
        {
            return StatusCode(401);
        }
        bool success = await _service.DeleteGroup(groupId);
        if (!success)
        {
            return StatusCode(400);
        }
        return StatusCode(200);
    }

    [HttpDelete(ApiRoutes.Group.RemoveUserFromGroup)]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> RemoveUserFromGroup(RemoveUserFromGroupDTO data)
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
        bool success = await _service.DeleteUserFromGroup(data.GroupId, data.UserId);
        if (!success)
        {
            return StatusCode(400);
        }
        return StatusCode(200);
    }

    [HttpPatch(ApiRoutes.Group.RegenGroupCode)]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> RegenGroupCode(GroupCodeRegenDTO data)
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
        Group? group = await _service.GetGroupAsync(data.GroupId);
        if (group == null)
        {
            return StatusCode(400);
        }
        if (user.Id != group.GroupOwner)
        {
            return StatusCode(401);
        }
        bool success = await _service.RegenGroupCode(data.GroupId);
        if (!success)
        {
            return StatusCode(400);
        }
        return StatusCode(200);
    }
}
