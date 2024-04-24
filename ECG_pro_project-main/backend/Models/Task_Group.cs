public class Task_Group
{
    public int TaskGroupId { get; set; }
    public int AssignedUserGroupId { get; set; }
    public int TaskAssignedId { get; set; }
    public int ECGDiagramId { get; set; }
    public bool Submitted { get; set; } = false;
    public virtual User_Group AssignedUserGroup { get; set; } = default!;
    public virtual Task TaskAssigned { get; set; } = default!;
    public virtual ECGDiagram ECGDiagramNavigation { get; set; } = default!;
    public virtual ICollection<Task_Question> TaskQuestions { get; set; } = default!;
}