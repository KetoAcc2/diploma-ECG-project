using System.ComponentModel.DataAnnotations;

public class Question_Type
{
    [Key]
    public int Question_TypeId { get; set; }
    public string QuestionTypeText { get; set; } = default!;
    public int RelatedQuestionId { get; set; }
    public virtual ICollection<ECGDiagram> ECGDiagramsNavigation { get; set; } = default!;
}