using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class User : IdentityUser<int>
{

    public User()
    {
        JoinedGroups = new HashSet<User_Group>();
        CreatedGroups = new HashSet<Group>();
    }
    public string? Password { get; set; } = default;
    public string Role { get; set; } = "Student";
    public byte[] UserSalt { get; set; } = default!;
    public byte[] UserHash { get; set; } = default!;
    public string? AccessToken { get; set; } = default;
    public string? RefreshToken { get; set; } = default;
    public string ActivationToken { get; set; } = Guid.NewGuid().ToString();
    public string ResetPasswordToken { get; set; } = "";
    public bool CanResetPassword { get; set; } = false;
    public DateTime? RefreshTokenExpiryTime { get; set; } = default;
    public DateTime? AccessTokenExpiryTime { get; set; } = default;

    public virtual ICollection<User_Group> JoinedGroups { get; set; }
    public virtual ICollection<Group> CreatedGroups { get; set; }
    public virtual ICollection<Task_Question> TaskQuestions { get; set; } = default!;
}

public class Role : IdentityRole<int>
{
    public Role() { }
    public Role(string roleName) : base(roleName) { }
}