using PostCrud.Dto;
using PostCrud.Model;

namespace PostCrud.Mappers;

public class PostMapper
{
    public static PostRespDto MapToDto(Post post)
    {
        return new PostRespDto
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            UserId = post.UserId
        };
    }

    public static Post MapToModel(PostDto postDto)
    {
        return new Post
        {
            Title = postDto.Title,
            Body = postDto.Body,
            UserId = postDto.UserId
        };
    }
}