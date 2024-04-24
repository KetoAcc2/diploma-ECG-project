using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace backend.Controllers;

[Authorize]
[ApiController]
[Route(ApiRoutes.User.MainRoute)]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;
    private readonly UserManager<User> _userManager;
    public UsersController(IUserService service, UserManager<User> userManager)
    {
        _service = service;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _service.GetUsersAsync());
    }

    [HttpGet("Dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? currentUser = await _service.GetUserByAccessToken(accessToken);
        if (currentUser == null)
        {
            return StatusCode(401);
        }
        return Ok(new UserDTO
        {
            UserId = currentUser.Id,
            Email = currentUser.Email,
            Role = currentUser.Role
        });
    }

    [HttpGet("{userId}")]
    [Authorize(Roles = UserRoleType.TEACHER)]
    public async Task<IActionResult> GetUser(int userId)
    {
        // var myUser = await _userManager.FindByIdAsync("1");
        // this.User.Identity.Name
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        UserDTO user = await _service.GetUserAsync(userId);
        User? currentUser = await _service.GetUserByAccessToken(accessToken);
        if (currentUser == null)
        {
            return StatusCode(401);
        }
        if (currentUser.Role.ToLower() != "teacher" && user.Email != currentUser.Email)
        {
            return StatusCode(401);
        }
        if (user != null)
        {
             
            return Ok(user);
        }
        return StatusCode(404);
    }

    [HttpGet("UsersFromGroup/{groupId}")]
    public async Task<IActionResult> GetUsersFromGroup(int groupId)
    {
        if (!await _service.ExistsGroupAsync(groupId))
        {
            return StatusCode(404);
        }
        return Ok(await _service.GetUsersFromGroupAsync(groupId));
    }

    [HttpGet(ApiRoutes.User.GetTeacherDoc)]
    [Authorize(Roles = UserRoleType.TEACHER)]
    public async Task<IActionResult> GetTeacherDoc()
    {
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? currentUser = await _service.GetUserByAccessToken(accessToken);
        if (currentUser == null)
        {
            return StatusCode(401);
        }
        if (currentUser.Role.ToLower() != "teacher")
        {
            return StatusCode(403);
        }
        return Ok(await _service.GetTeacherDoc());
    }

    [HttpGet(ApiRoutes.User.GetStudentDoc)]
    public async Task<IActionResult> GetStudentDoc()
    {
        return Ok(await _service.GetStudentDoc());
    }
}
