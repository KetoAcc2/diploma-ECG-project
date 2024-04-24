using System.ComponentModel.DataAnnotations;

[Serializable]
public class GroupDTO
{
    [Key]
    public int GroupId { get; set; }
    public String GroupName { get; set; } = default!;
    public string GroupCode { get; set; } = default!;

}