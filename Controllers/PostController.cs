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

    public PostController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        List<Post> allPosts = await _postRepository.GetAllPosts();
        var dtoList = allPosts.Select(post => PostMapper.MapToDto(post));
        return Ok(dtoList);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetPostById([FromRoute] int id)
    {
        return Ok(PostMapper.MapToDto(await _postRepository.GetPostById(id)));
    }

    [HttpGet]
    [Route("filter")]
    public async Task<IActionResult> FilterPosts([FromQuery] PostFilterDto filterDto)
    {
        var posts = await _postRepository.FilterPosts(filterDto);
        var dtoList = posts.Select(p => PostMapper.MapToDto(p));
        return Ok(dtoList);
    }
    
    [HttpGet]
    [Route("paged/filter")]
    public async Task<IActionResult> FilterPostsWithPage([FromQuery] PostFilterDto filterDto, [FromQuery] PageDto pageDto)
    {
        var posts = await _postRepository.FilterPostsWithPage(filterDto, pageDto);
        var dtoList = posts.Select(p => PostMapper.MapToDto(p));
        return Ok(dtoList);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] PostDto postDto)
    {
        var resp = PostMapper.MapToDto(await _postRepository.Create(postDto));
        return CreatedAtAction(nameof(GetPostById), new { id = resp.Id }, resp);
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdatePost([FromRoute] int id, [FromBody] PostDto postDto)
    {
        return Ok(PostMapper.MapToDto(await _postRepository.Update(id, postDto)));
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await _postRepository.Delete(id);
        return NoContent();
    }
}