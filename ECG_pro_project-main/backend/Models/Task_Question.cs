using System.ComponentModel.DataAnnotations;

public class Task_Question
{
    [Key]
    public int TaskQuestionId { get; set; }
    public int BelongedTaskGroupId { get; set; }
    public int UserId { get; set; }
    public int ParentQuestionNumber { get; set; }
    public int QuestionNumber { get; set; }
    public int AnswerNumber { get; set; }
    public string Answer { get; set; } = default!;
    public virtual Task_Group BelongedTaskGroup { get; set; } = default!;
    public virtual User UserNavigation { get; set; } = default!;
}