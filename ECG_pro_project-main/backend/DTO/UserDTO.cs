using System.ComponentModel.DataAnnotations;

public class UserDTO
{

    [Key]
    public int UserId { get; set; }
    public String Email { get; set; } =default!;
    public String Role { get; set; }=default!;

}