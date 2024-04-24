public class TaskContentDTO
{
    public int TaskId { get; set; }
    public List<QuestionDTO> QuestionsRelatedToThisTask { get; set; } = default!;
}