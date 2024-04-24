using System.ComponentModel.DataAnnotations;
public class Group
{
    //TODO: some error here but idk what to do about it
    public Group()
    {
        JoinedGroups = new HashSet<User_Group>();
    }
    [Key]
    public int GroupId { get; set; }
    // public string GroupName { get; set; }
    public int GroupOwner { get; set; }
    public string GroupName { get; set; } = default!;
    public string GroupCode { get; set; } = default!;
    public DateTime CreatedTime { get; set; } = DateTime.Now;
    public virtual ICollection<User_Group> JoinedGroups { get; set; }
    public virtual ICollection<Task_Group> TasksForGroup { get; set; } = default!;
    public virtual User IdGroupOwnerNavigation { get; set; } = default!;
}