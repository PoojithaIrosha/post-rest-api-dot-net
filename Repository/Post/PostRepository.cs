using Microsoft.EntityFrameworkCore;
using PostCrud.Config;
using PostCrud.Dto;
using PostCrud.Exception;
using PostCrud.Interface;
using PostCrud.Mappers;
using PostCrud.Model;

namespace PostCrud.Repository;

public class PostRepository : IPostRepository

{
    private readonly AppDbContext _context;

    public PostRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Post>> GetAllPosts()
    {
        return await _context.Posts.ToListAsync();
    }

    public async Task<Post> GetPostById(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null)
        {
            throw new PostNotFoundException(id);
        }

        return post;
    }

    public async Task<Post> Create(PostDto postDto)
    {
        var user = await _context.Users.FindAsync(postDto.UserId);
        if (user == null)
        {
            throw new UserNotFoundException(postDto.UserId);
        }

        var post = PostMapper.MapToModel(postDto);

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        return post;
    }

    public async Task<Post> Update(int id, PostDto postDto)
    {
        var existingPost = await _context.Posts.FindAsync(id);
        var user = await _context.Users.FindAsync(postDto.UserId);

        if (existingPost == null)
        {
            throw new PostNotFoundException(id);
        }

        if (user == null)
        {
            throw new UserNotFoundException(postDto.UserId);
        }

        existingPost.Title = postDto.Title;
        existingPost.Body = postDto.Body;
        existingPost.UserId = postDto.UserId;

        await _context.SaveChangesAsync();

        return existingPost;
    }

    public async Task<bool> Delete(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null)
        {
            throw new PostNotFoundException(id);
        }

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return true;
    }

    public IQueryable<Post> FilterPostsQuery(PostFilterDto filterDto)
    {
        var stocks = _context.Posts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filterDto.Search))
        {
            stocks = stocks.Where(s => s.Title.ToLower().Contains(filterDto.Search.ToLower()));
        }

        if (filterDto.UserId.HasValue)
        {
            stocks = stocks.Where(s => s.UserId == filterDto.UserId);
        }

        if (!string.IsNullOrWhiteSpace(filterDto.Sort))
        {
            stocks = filterDto.Sort.ToLower() switch
            {
                "id" => filterDto.IsAscending
                    ? stocks.OrderBy(s => s.Id)
                    : stocks.OrderByDescending(s => s.Id),
                "title" => filterDto.IsAscending
                    ? stocks.OrderBy(s => s.Title)
                    : stocks.OrderByDescending(s => s.Title),
                "body" => filterDto.IsAscending
                    ? stocks.OrderBy(s => s.Body)
                    : stocks.OrderByDescending(s => s.Body),
                _ => stocks
            };
        }

        return stocks;
    }
    
    public async Task<List<Post>> FilterPosts(PostFilterDto filterDto)
    {
        return await FilterPostsQuery(filterDto).ToListAsync();
    }
    public async Task<List<Post>> FilterPostsWithPage(PostFilterDto filterDto, PageDto pageDto)
    {
        var query = FilterPostsQuery(filterDto);

        var skipNumber = (pageDto.Page - 1) * pageDto.Size;
        query = query.Skip(skipNumber).Take(pageDto.Size);

        return await query.ToListAsync();
    }
}