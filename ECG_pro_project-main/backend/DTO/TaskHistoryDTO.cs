public class TaskHistoryDTO
{
    public int TaskId { get; set; }
    public string TaskDescription { get; set; } = default!;
    public double TaskScore { get; set; }
    public int GroupId { get; set; }
    public string GroupName { get; set; }
}