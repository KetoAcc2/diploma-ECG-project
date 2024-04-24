using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

[Authorize(Roles = UserRoleType.PSEUDO_ADMIN)]
[ApiController]
[Route(ApiRoutes.PseudoAdmin.MainRoute)]
[Produces("application/json")]
public class PseudoAdminController : ControllerBase
{
    private readonly IPseudoAdminService _service;
    public PseudoAdminController(IPseudoAdminService service)
    {
        _service = service;
    }

    [HttpPost(ApiRoutes.PseudoAdmin.UpdateRole)]
    public async Task<IActionResult> UpdateRole(RoleUpdateDTO data)
    {

        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? user = await _service.GetUserByAccessToken(accessToken);
        if (user == null)
        {
            return StatusCode(401);
        }
        User? userByEmail = await _service.GetUserByEmail(data.Email);
        if (userByEmail == null)
        {
            return StatusCode(404);
        }
        if (userByEmail.Role == UserRoleType.PSEUDO_ADMIN)
        {
            return StatusCode(403);
        }
        if (!await _service.UpdateRole(data.Email, data.Role))
        {
            return StatusCode(400);
        }
        return StatusCode(200);
    }

    [HttpGet(ApiRoutes.PseudoAdmin.GetITDoc)]
    public async Task<IActionResult> GetITDoc()
    {
        return Ok(await _service.GetITDoc());
    }

    [HttpGet(ApiRoutes.PseudoAdmin.GetUserRole)]
    public async Task<IActionResult> GetUserRole(string email)
    {
        string accessToken = Utils.getAccessToken(Request.Headers[HeaderNames.Authorization]);
        User? user = await _service.GetUserByAccessToken(accessToken);
        if (user == null)
        {
            return StatusCode(401);
        }
        User? userByEmail = await _service.GetUserByEmail(email);
        if (userByEmail == null)
        {
            return StatusCode(404);
        }
        if (userByEmail.Role == UserRoleType.PSEUDO_ADMIN)
        {
            return StatusCode(403);
        }
        return Ok(_service.GetUserRole(email));
    }
}