public class FinishedTasksByGroupDTO
{
    public int GroupId { get; set; }
    public List<FinishedTasksByGroup> FinishedTasks { get; set; } = default!;

}

public class FinishedTasksByGroup
{
    public int TaskId { get; set; }
    public string TaskDescription { get; set; } = default!;
    public double TaskScore { get; set; }
    public DateTime CreatedTime { get; set; }
}