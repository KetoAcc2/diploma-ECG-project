

using System.ComponentModel.DataAnnotations;

public class UserRegisterDTO
{
    [Required(ErrorMessage ="Email is required.")]
    [RegularExpression(@"^\w+([-+.']\w+)*@((?:gmail.com)|(?:student.wum.edu.pl)|(?:wum.edu.pl))$", ErrorMessage ="Invalid Email.")]
    public String Email { get; set; } = default!;
    public String Password { get; set; } = default!;

}