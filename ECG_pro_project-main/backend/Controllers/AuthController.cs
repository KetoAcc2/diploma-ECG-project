using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace backend.Controllers;

[ApiController]
[Route(ApiRoutes.Auth.MainRoute)]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IJwtTokenService _service;
    private readonly IEmailService _emailService;
    private readonly UserManager<User> _userManager;
    public AuthController(IJwtTokenService service, IEmailService emailService, UserManager<User> userManager)
    {
        _service = service;
        _emailService = emailService;
        _userManager = userManager;
    }


    [HttpPost(ApiRoutes.Auth.Login)]
    public async Task<IActionResult> Login(UserLoginDTO request)
    {

        if (!await _service.ExistsUserByEmailAsync(request.Email))
        {
            return StatusCode(401);
        }

        if (!await _service.AccountActivated(request.Email))
        {
            return StatusCode(401);
        }
        var hash = await _service.GetUserHash(request.Email);
        var salt = await _service.GetUserSalt(request.Email);
        if (hash == null || salt == null)
        {
            return StatusCode(401);
        }
        if (!_service.VerifyPasswordHash(request.Password, hash, salt))
        {
            return StatusCode(401);
        }
        JwtTokenDTO? jwt = await _service.RenewAccessRefreshToken(request.Email);
        if (jwt == null)
        {
            return StatusCode(500);
        }
        UserDTO? userData = await _service.GetUserByEmailReturnDTO(request.Email);
        if (userData == null)
        {
            return StatusCode(500);
        }
        return Ok(new LoginResponseDataDTO
        {
            UserData = userData,
            JwtToken = jwt
        });
    }
    [HttpPost(ApiRoutes.Auth.Register)]
    public async Task<IActionResult> Register(UserRegisterDTO request)
    {
         

        if (!await _service.RegisterNewUser(request))
        {
             
            return StatusCode(500);
        }

        return Ok("New account registered");
    }
    [HttpPost(ApiRoutes.Auth.Refresh)]
    public async Task<IActionResult> RenewAccessToken(JwtTokenDTO request)
    {
        if (request is null)
        {
            return StatusCode(400);
        }
        User? user = await _service.GetUserByAccessToken(request.AccessToken);
        if (user is null)
        {
            return StatusCode(401);
        }
        if (user.RefreshToken != request.RefreshToken)
        {
            return StatusCode(403);
        }
        if (user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return StatusCode(401);
        }
        return Ok(new
        {
            UserData = new UserDTO { UserId = user.Id, Email = user.Email, Role = user.Role },
            AccessToken = _service.CreateAccessToken(user.Email)
        });
    }

    [Authorize]
    [HttpPost(ApiRoutes.Auth.Logout)]
    public async Task<IActionResult> Logout(string email)
    {
        if (await _service.Logout(email))
        {
            return StatusCode(200);
        }
        return StatusCode(400);
    }

    [HttpPost(ApiRoutes.Auth.ActivateEmail)]
    public async Task<IActionResult> ActivateEmail(string activationToken)
    {
        User? user = await _emailService.GetUserByActivationToken(activationToken);

        if (user == null)
        {
            return StatusCode(401);
        }
        if (await _emailService.EmailIsActivated(activationToken))
        {
            return StatusCode(304);
        }
        if (!await _emailService.ActivateEmail(activationToken))
        {
            return StatusCode(400);
        }
        return StatusCode(200);
    }
    [HttpPost(ApiRoutes.Auth.ResetPassowrd)]
    public async Task<IActionResult> ResetPassword(string resetPasswordToken, ResetPasswordDTO data)
    {
        User? user = await _emailService.GetUserByResetPasswordToken(resetPasswordToken);
        if (user == null)
        {
            return StatusCode(401);
        }
        if (!await _service.ResetUserPassword(user.Id, data.Password))
        {
            return StatusCode(400);
        }
        return StatusCode(200);
    }

    [HttpPost(ApiRoutes.Auth.ResetPasswordSendEmail)]
    public async Task<IActionResult> ResetPasswordSendEmail(ResetPasswordEmail data)
    {
        User? user = await _emailService.GetUserByEmail(data.Email);
        if (user == null)
        {
            return StatusCode(401);
        }
        if (!await _service.SetResetPasswordToken(user.Id))
        {
            return StatusCode(400);
        }
        string? resetPasswordToken = await _service.GetResetPasswordToken(user.Id);
        if (resetPasswordToken == null)
        {
            return StatusCode(400);
        }
        if (!await _emailService.SendEmail(new EmailTemplate
        {
            EmailTo = data.Email,
            EmailSubject = "Reset password/Resetowanie hasla",
            EmailBody = $"We received a request to reset the password for your account. If you made this request, please follow the instructions below to reset your password.\r\n\r\nTo reset your password, please click on the link below:\r\n\r\n{ApiRoutes.baseURL}resetpassword/{resetPasswordToken}\r\n\r\nIf you did not request a password reset, please ignore this email.\r\n\r\n\r\n\r\nOtrzymaliśmy prośbę o zresetowanie hasła do Twojego konta. Jeśli złożyli Państwo taką prośbę, prosimy o postępowanie zgodnie z poniższymi instrukcjami, aby zresetować hasło.\r\n\r\nAby zresetować hasło, proszę kliknąć na poniższy link:\r\n\r\n{ApiRoutes.baseURL}resetpassword/{resetPasswordToken}Jeśli nie złożyłeś prośby o zresetowanie hasła, prosimy o zignorowanie tej wiadomości."
        }))
        {
            return StatusCode(400);
        }
        return StatusCode(200);
    }

}
