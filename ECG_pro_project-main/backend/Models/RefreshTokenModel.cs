public class RefreshTokenModel
{
    public string RefreshToken { get; set; } = default!;
    public DateTime RefreshTokenExpiryTime { get; set; } = default;
}