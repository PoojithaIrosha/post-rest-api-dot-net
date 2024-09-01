using Microsoft.AspNetCore.Mvc;
using PostCrud.Dto.Auth;
using PostCrud.Repository.User;
using PostCrud.Services;

namespace PostCrud.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController: Controller
{

    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    
    public AuthController(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginReqDto reqDto)
    {
        var user = await _userRepository.FindByUsername(reqDto.Username);

        if (!BCrypt.Net.BCrypt.Verify(reqDto.Password, user.Password))
        {
            return Unauthorized("Username or Password is Incorrect");
        }

        AuthResp resp = new AuthResp
        {
            UserId = user.Id,
            Username = user.Username,
            Token = _tokenService.CreateToken(user)
        };
        
        return Ok(resp);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var user = await _userRepository.SaveUser(dto);
        var token = _tokenService.CreateToken(user);

        var resp = new AuthResp
        {
            Username = user.Username,
            UserId = user.Id,
            Token = token
        };
        
        return Ok(resp);
    }
    
}