public class OverallScorePerGroupDTO
{
    public List<ScorePerGroup> ScorePerGroups { get; set; } = default!;
}

public class ScorePerGroup{
    public int GroupId { get; set; }
    public double ScoreInPercentage { get; set; }
}