namespace PostCrud.Dto;

public class PostFilterDto
{
    public string? Search { get; set; }
    public int? UserId { get; set; }
    public string? Sort { get; set; }
    public bool IsAscending { get; set; } = true;
}