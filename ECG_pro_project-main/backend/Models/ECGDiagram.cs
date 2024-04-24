
public class ECGDiagram
{
    public int ECGDiagramId { get; set; }
    public string Image { get; set; } = default!;
    public int QuestionTypeId { get; set; }
    public int Komor { get; set; }
    public int PR { get; set; }
    public int PQ { get; set; }
    public int QT { get; set; }
    public int QTC { get; set; }
    public virtual ICollection<CheatSheet> CheatSheets { get; set; } = default!;
    public virtual ICollection<Task_Group> TaskGroups { get; set; } = default!;
    public virtual Question_Type QuestionType { get; set; } = default!;
}