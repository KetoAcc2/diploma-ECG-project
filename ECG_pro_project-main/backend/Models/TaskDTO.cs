public class TaskDTO
{
    public int TaskId { get; set; }
    public string TaskDescription { get; set; } = default!;
    public int GroupId { get; set; }
    public string GroupName { get; set; } = default!;
}