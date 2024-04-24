public class QuestionDTO
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = default!;
    public List<AnswerDTO> AvailableAnswers { get; set; } = default!;
}