public class TaskHistoryResponseDTO
{
    public List<AnswerCheckStructure> CheatSheets { get; set; } = default!;
    public List<AnswerCheckStructure> UserAnswers { get; set; } = default!;
}

public class AnswerCheckStructure
{
    public int ParentQuestionNumber { get; set; }
    public int QuestionNumber { get; set; }
    public int AnswerNumber { get; set; }
    public string Answer { get; set; } = default!;
}