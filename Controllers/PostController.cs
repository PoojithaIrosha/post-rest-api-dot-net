using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PostCrud.Dto;
using PostCrud.Interface;
using PostCrud.Mappers;
using PostCrud.Model;

namespace PostCrud.Controllers;

[ApiController]
[Route("api/posts")]
[Authorize(Roles = "USER")]
public class PostController : Controller
{
    private readonly IPostRepository _postRepository;
    private readonly ILogger<PostController> _logger;

    public PostController(IPostRepository postRepository, ILogger<PostController> logger)
    {
        _postRepository = postRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        _logger.LogInformation("START: Getting All Posts");

        List<Post> allPosts = await _postRepository.GetAllPosts();
        var dtoList = allPosts.Select(post => PostMapper.MapToDto(post));

        _logger.LogInformation("END: Getting All Posts Success");
        return Ok(dtoList);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetPostById([FromRoute] int id)
    {
        _logger.LogInformation("START: Get Post By ID {id}", id);
        var post = await _postRepository.GetPostById(id);
        _logger.LogInformation("END: Get Post By ID {id}", id);
        return Ok(PostMapper.MapToDto(post));
    }

    [HttpGet]
    [Route("filter")]
    public async Task<IActionResult> FilterPosts([FromQuery] PostFilterDto filterDto)
    {
        _logger.LogInformation("START: Get Posts with Filters: {filters}", filterDto);

        var posts = await _postRepository.FilterPosts(filterDto);
        var dtoList = posts.Select(p => PostMapper.MapToDto(p));

        _logger.LogInformation("END: Get Posts with Filters: {filters}", filterDto);
        return Ok(dtoList);
    }

    [HttpGet]
    [Route("paged/filter")]
    public async Task<IActionResult> FilterPostsWithPage([FromQuery] PostFilterDto filterDto,
        [FromQuery] PageDto pageDto)
    {
        _logger.LogInformation("START: Get Posts with Filters & Pagination: {filters}, Page: {page}", filterDto,
            pageDto);

        var posts = await _postRepository.FilterPostsWithPage(filterDto, pageDto);
        var dtoList = posts.Select(p => PostMapper.MapToDto(p));

        _logger.LogInformation("END: Get Posts with Filters & Pagination: {filters}, Page: {page}", filterDto, pageDto);
        return Ok(dtoList);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] PostDto postDto)
    {
        _logger.LogInformation("START: Create Post with Title: {title}", postDto.Title);

        var resp = PostMapper.MapToDto(await _postRepository.Create(postDto));

        _logger.LogInformation("END: Post created with ID {id}", resp.Id);

        return CreatedAtAction(nameof(GetPostById), new { id = resp.Id }, resp);
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdatePost([FromRoute] int id, [FromBody] PostDto postDto)
    {
        _logger.LogInformation("START: Update Post By ID {id}", id);
        
        var post = await _postRepository.Update(id, postDto);
        
        _logger.LogInformation("END: Update Post By ID {id}", id);
        return Ok(PostMapper.MapToDto(post));
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        _logger.LogInformation("START: Delete Post By ID {id}", id);
        
        await _postRepository.Delete(id);
        
        _logger.LogInformation("START: Delete Post By ID {id}", id);
        return NoContent();
    }
}