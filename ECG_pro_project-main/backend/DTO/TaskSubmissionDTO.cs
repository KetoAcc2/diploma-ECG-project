public class TaskSubmissionDTO
{
    public int TaskId { get; set; }
    public AnswerStructure[] AnswerStructures { get; set; } = default!;
}

public class AnswerStructure
{
    public int ParentQuestionNumber { get; set; }
    public int QuestionNumber { get; set; }
    public int AnswerNumber { get; set; }
    public int EcgDiagramId { get; set; }
    public string Answer { get; set; } = default!;
}