using System.ComponentModel.DataAnnotations;

public class User_Group
{
    [Key]
    public int User_GroupId { get; set; } = default;
    public int UserId { get; set; } = default;
    public int GroupId { get; set; } = default;
    public virtual User IdUserNavigation { get; set; } = default!;
    public virtual Group IdGroupNavigation { get; set; } = default!;
    public virtual ICollection<Task_Group> TaskGroups { get; set; } = default!;
}