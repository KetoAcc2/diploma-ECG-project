using Microsoft.EntityFrameworkCore;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;


public class EmailServices : BaseService, IEmailService
{
    private readonly ApplicationDbContext _db;
    public EmailServices(ApplicationDbContext context) : base(context)
    {
        _db = context;
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
    public async Task<bool> EmailIsActivated(string activationToken)
    {
        User? user = await _db.Users.Where(x => x.ActivationToken == activationToken).SingleOrDefaultAsync();
        if (user == null)
        {
            return false;
        }
        return user.EmailConfirmed;
    }
    public async Task<User?> GetUserByActivationToken(string activationToken)
    {
        return await _db.Users.Where(x => x.ActivationToken == activationToken).SingleOrDefaultAsync();
    }
    public async Task<bool> ActivateEmail(string activationToken)
    {
        User? user = await _db.Users.Where(x => x.ActivationToken == activationToken).SingleOrDefaultAsync();
        if (user == null)
        {
            return false;
        }
        user.EmailConfirmed = true;
        await _db.SaveChangesAsync();
        return true;
    }
    public async Task<User?> GetUserByResetPasswordToken(string resetPasswordToken)
    {
        return await _db.Users.Where(x => x.ResetPasswordToken == resetPasswordToken).SingleOrDefaultAsync();
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
