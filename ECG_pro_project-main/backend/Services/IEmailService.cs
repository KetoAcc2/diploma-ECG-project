public interface IEmailService : IBaseService
{
    Task<bool> SendEmail(EmailTemplate data);
    Task<bool> EmailIsActivated(string activationToken);
    Task<bool> ActivateEmail(string activationToken);
    Task<User?> GetUserByActivationToken(string activationToken);
    Task<User?> GetUserByResetPasswordToken(string resetPasswordToken);
    
}