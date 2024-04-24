public class CheatSheet
{
    public int CheatSheetId { get; set; }
    public int ParentQuestionNumber { get; set; }
    public int QuestionNumber { get; set; }
    public int AnswerNumber { get; set; }
    public int ECGDiagramId { get; set; }
    public string Answer { get; set; } = default!;
    public virtual ECGDiagram ECGDiagramNavigation { get; set; } = default!;
}