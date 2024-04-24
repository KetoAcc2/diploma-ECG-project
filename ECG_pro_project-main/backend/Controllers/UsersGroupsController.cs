using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace backend.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class UsersGroupsController : Controller
{
    private readonly IUserGroupService _service;
    public UsersGroupsController(IUserGroupService service){
        _service = service;
    }
    [HttpGet("GroupsInfo")]
    public async Task<IActionResult> GetGroupsInfo()
    {
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? user = await _service.GetUserByAccessToken(accessToken);
        if(user == null){
            return StatusCode(401);
        }
        return Ok(await _service.GetGroupInfoFromUser(user.Id));
    }
}