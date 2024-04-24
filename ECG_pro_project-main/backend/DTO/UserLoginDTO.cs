using System.ComponentModel.DataAnnotations;

public class UserLoginDTO
{
    public String Email { get; set; } = default!;
    public String Password { get; set; } = default!;

}