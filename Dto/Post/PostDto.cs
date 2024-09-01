using System.ComponentModel.DataAnnotations;

namespace PostCrud.Dto;

public class PostDto
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Body { get; set; }
    [Required]
    public int UserId { get; set; }
}