public class LoginResponseDataDTO
{
    public UserDTO UserData { get; set; } = default!;
    public JwtTokenDTO JwtToken { get; set; } = default!;
    
}