using PostCrud.Dto;
using PostCrud.Model;

namespace PostCrud.Interface;

public interface IPostRepository
{
    Task<List<Post>> GetAllPosts();

    Task<Post> GetPostById(int id);
    Task<Post> Create(PostDto postDto);
    Task<Post> Update(int id, PostDto postDto);
    Task<bool> Delete(int id);

    Task<List<Post>> FilterPosts(PostFilterDto filterDto);
    Task<List<Post>> FilterPostsWithPage(PostFilterDto filterDto, PageDto pageDto);
}