public class Task
{
    public int TaskId { get; set; }
    public string TaskDescription { get; set; } = default!;
    public DateTime TimeCreated { get; set; } = DateTime.Now;
    public DateTime? EndTime { get; set; }
    public virtual ICollection<Task_Group> GroupsAssignedWithThisTask { get; set; } = default!;
}