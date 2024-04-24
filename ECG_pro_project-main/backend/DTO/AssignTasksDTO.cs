public class AssignTasksDTO
{
    //Tasks are actually QuestionTypes
    public int[] Tasks { get; set; } = default!;
    public int[] Groups { get; set; } = default!;
    public string TaskDescription { get; set; } = DateTime.Now.ToString();
}