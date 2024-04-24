using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

public class JwtTokenService : BaseService, IJwtTokenService
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    public JwtTokenService(ApplicationDbContext db, IConfiguration configuration, UserManager<User> userManager) : base(db)
    {
        _db = db;
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<bool> ExistsUserAsync(int userId)
    {
        return await _db.Users.AnyAsync(user => user.Id == userId);
    }
    public async Task<bool> ExistsUserByEmailAsync(string email)
    {
        return (await _userManager.FindByEmailAsync(email)) == null ? true : false;
        // return await _db.Users.AnyAsync(user => user.Email == email);
    }
    public async Task<JwtTokenDTO?> RenewAccessRefreshToken(string email)
    {
        var user = await GetUserByEmailAsync(email);
        if (user == null)
        {
            return null;
        }

        var newAccessTokenModel = CreateAccessToken(email);
        var newRefreshTokenModel = GenerateRefreshToken();
        user.AccessToken = newAccessTokenModel.AccessToken;
        user.AccessTokenExpiryTime = newAccessTokenModel.AccessTokenExpiryTime;
        user.RefreshToken = newRefreshTokenModel.RefreshToken;
        user.RefreshTokenExpiryTime = newRefreshTokenModel.RefreshTokenExpiryTime;
        await _db.SaveChangesAsync();
        return new JwtTokenDTO
        {
            AccessToken = newAccessTokenModel.AccessToken,
            RefreshToken = newRefreshTokenModel.RefreshToken
        };
    }
    public AccessTokenModel CreateAccessToken(string email)
    {
        var user = _db.Users.Where(x => x.Email == email).Single();

        List<Claim> claims = new List<Claim>{
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, user != null ? user.Role : "blank")
        };
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var expiryTime = DateTime.Now.AddHours(3);
        var token = new JwtSecurityToken(
            audience: _configuration["JWT:ValidAudience"],
            issuer: _configuration["JWT:ValidIssuer"],
            claims: claims,
            expires: expiryTime,
            signingCredentials: credentials
        );
        var newAccessToken = new JwtSecurityTokenHandler().WriteToken(token);
        user.AccessToken = newAccessToken;
        user.AccessTokenExpiryTime = expiryTime;
        _db.SaveChanges();
        return new AccessTokenModel
        {
            AccessToken = newAccessToken,
            AccessTokenExpiryTime = expiryTime
        };
    }
    public RefreshTokenModel GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var newRefreshToken = Convert.ToBase64String(randomNumber);

        return new RefreshTokenModel
        {
            RefreshToken = newRefreshToken,
            RefreshTokenExpiryTime = DateTime.Now.AddHours(12)
        };
    }

    public async Task<byte[]> GetUserHash(string email)
    {
        var user = await _db.Users.Where(x => x.Email == email).SingleOrDefaultAsync();
        if (user != null)
        {
            return user.UserHash;
        }
        return new byte[0];
    }
    public async Task<User?> GetUserByEmailReturnUser(string email)
    {
        return await _db.Users.Where(x => x.Email == email).SingleOrDefaultAsync();
    }
    public async Task<byte[]> GetUserSalt(string email)
    {
        var user = await _db.Users.Where(x => x.Email == email).SingleOrDefaultAsync();
        if (user != null)
        {
            return user.UserSalt;
        }
        return new byte[0];
    }
    public async Task<bool> AccountActivated(string email)
    {
        var user = await _db.Users.Where(x => x.Email == email).SingleOrDefaultAsync();
        if (user == null)
        {
            return false;
        }
         
        return user.EmailConfirmed;
    }
    public async Task<bool> RegisterNewUser(UserRegisterDTO request)
    {
        using var transaction = await _db.Database.BeginTransactionAsync();
        var user = await GetUserByEmailAsync(request.Email);
        if (user != null)
        {
            return false;
        }
        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
        try
        {
            var newAccessTokenModel = CreateAccessToken(request.Email);
            var newRefreshTokenModel = GenerateRefreshToken();
             
            var emailArr = request.Email.Trim().Split("@");
            User newUser = new User
            {
                Email = request.Email,
                UserSalt = passwordSalt,
                UserHash = passwordHash,
                // Role = emailArr[1] == "wum.edu.pl"?UserRoleType.TEACHER : UserRoleType.STUDENT,
                // ask doctor if we can set teacher's role by using wum.edu.pl to distinguish
                // Role = "blank", //role should be nullable or set default value, need to decide which approach at some point
                AccessToken = newAccessTokenModel.AccessToken,
                RefreshToken = newRefreshTokenModel.RefreshToken,
                AccessTokenExpiryTime = newAccessTokenModel.AccessTokenExpiryTime,
                RefreshTokenExpiryTime = newRefreshTokenModel.RefreshTokenExpiryTime,
                ActivationToken = Guid.NewGuid().ToString()
            };
            await SaveUserAsync(newUser);
            if (!await SendEmail(new EmailTemplate
            {
                EmailTo = request.Email,
                EmailSubject = "Account activation/Aktywacja konta",
                EmailBody = $"Dziękujemy za założenie konta w naszym serwisie. Aby aktywować swoje konto, proszę kliknąć poniższy link:\r\n\r\n{ApiRoutes.baseURL}Auth/Activate/{newUser.ActivationToken}\r\n\r\nJeśli nie możesz kliknąć linku, możesz również skopiować i wkleić go do paska adresu przeglądarki.\r\n\r\nJeśli nie prosiłeś o utworzenie konta, proszę zignorować ten e-mail.\r\nPozdrawiamy,\r\r\nECG app\r\n\r\n\r\n\r\n\r\n Thank you for creating an account with our service. To activate your account, please click the link below:\r\n\r\n{ApiRoutes.baseURL}Auth/Activate/{newUser.ActivationToken}\r\n\r\nIf you are unable to click the link, you can also copy and paste it into your browser's address bar.\r\n\r\nIf you did not request to create an account, please ignore this email.\r\n\r\nBest regards,\r\nECG app"
            }))
            {
                await transaction.RollbackAsync();
                return false;
            }
            await transaction.CommitAsync();
        }
        catch (System.Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }
        return true;
    }
    private async Task<bool> SaveUserAsync(User user)
    {
        try
        {
            await _db.AddAsync(user);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }
    private async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _db.Users.Where(x => x.Email == email).SingleOrDefaultAsync();
    }
    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }


    public async Task<bool> Logout(string email)
    {
        try
        {
            var user = await _db.Users.Where(x => x.Email == email).SingleOrDefaultAsync();
            if (user is null)
            {
                return false;
            }
            user.RefreshToken = "";
            user.RefreshTokenExpiryTime = null;
            await _db.SaveChangesAsync();
        }
        catch (System.Exception)
        {

            return false;
        }
        return true;
    }
    public async Task<bool> ResetUserPassword(int userId, string password)
    {
        User? user = await _db.Users.Where(x => x.Id == userId).SingleOrDefaultAsync();
        if (user == null)
        {
            return false;
        }
        try
        {
            user.ResetPasswordToken = "";
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.UserHash = passwordHash;
            user.UserSalt = passwordSalt;
            await _db.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> SetResetPasswordToken(int userId)
    {
        try
        {
            User? user = await _db.Users.Where(x => x.Id == userId).SingleOrDefaultAsync();
            if (user == null)
            {
                return false;
            }
            user.ResetPasswordToken = Guid.NewGuid().ToString();
            await _db.SaveChangesAsync();
            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }

    public async Task<string?> GetResetPasswordToken(int userId)
    {
        return await _db.Users.Where(x => x.Id == userId).Select(x => x.ResetPasswordToken).SingleOrDefaultAsync();
    }


    public async Task<bool> SendEmail(EmailTemplate data)
    {
        try
        {
            string[] Scopes = { GmailService.Scope.GmailSend };
            UserCredential credential;
            using (var stream = new FileStream(
                "./client_secret.json",
                FileMode.Open,
                FileAccess.Read
            ))
            {
                string credPath = "token_Send.json";
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                             GoogleClientSecrets.FromStream(stream).Secrets,
                              Scopes,
                              "user",
                              CancellationToken.None,
                              new FileDataStore(credPath, true));
            }
            // Create Gmail API service.
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "ecg",
            });
            //Parsing HTML 
            string message = $"To: {data.EmailTo}\r\nSubject: {data.EmailSubject}\r\nContent-Type: text/plain;charset=utf-8\r\n\r\n{data.EmailBody}";
            var newMsg = new Google.Apis.Gmail.v1.Data.Message();
            newMsg.Raw = this.Base64UrlEncode(message.ToString());
            Message response = await service.Users.Messages.Send(newMsg, "me").ExecuteAsync();

            if (response != null)
                return true;
            else
                return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
    private string Base64UrlEncode(string input)
    {
        var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
        // Special "url-safe" base64 encode.
        return Convert.ToBase64String(inputBytes)
          .Replace('+', '-')
          .Replace('/', '_')
          .Replace("=", "");
    }
}