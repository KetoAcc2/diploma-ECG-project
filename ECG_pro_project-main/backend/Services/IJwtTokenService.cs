public interface IJwtTokenService : IBaseService
{
    Task<byte[]> GetUserHash(string email);
    Task<byte[]> GetUserSalt(string email);
    bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    AccessTokenModel CreateAccessToken(string email);
    Task<bool> RegisterNewUser(UserRegisterDTO request);
    Task<bool> ExistsUserAsync(int userId);
    Task<bool> ExistsUserByEmailAsync(string email);
    RefreshTokenModel GenerateRefreshToken();
    Task<JwtTokenDTO?> RenewAccessRefreshToken(string email);
    Task<bool> Logout(string email);
    Task<bool> AccountActivated(string email);
    Task<User?> GetUserByEmailReturnUser(string email);
    Task<bool> ResetUserPassword(int userId, string password);
    Task<bool> SetResetPasswordToken(int userId);
    Task<string?> GetResetPasswordToken(int userId);
}