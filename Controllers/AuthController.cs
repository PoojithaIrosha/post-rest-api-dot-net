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
    private readonly ILogger<AuthController> _logger;
    
    public AuthController(IUserRepository userRepository, ITokenService tokenService, ILogger<AuthController> logger)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginReqDto reqDto)
    {
        _logger.LogInformation("START: Login attempt received with username {username}", reqDto.Username);
        var user = await _userRepository.FindByUsername(reqDto.Username);

        if (!BCrypt.Net.BCrypt.Verify(reqDto.Password, user.Password))
        {
            _logger.LogInformation("ERROR: Login failed with username {username}", reqDto.Username);
            return Unauthorized("Username or Password is Incorrect");
        }

        AuthResp resp = new AuthResp
        {
            UserId = user.Id,
            Username = user.Username,
            Token = _tokenService.CreateToken(user)
        };
        _logger.LogInformation("END: Login success with username {username}", reqDto.Username);
        return Ok(resp);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        _logger.LogInformation("START: Register attempt received with username {username}", dto.Username);
        var user = await _userRepository.SaveUser(dto);
        var token = _tokenService.CreateToken(user);

        var resp = new AuthResp
        {
            Username = user.Username,
            UserId = user.Id,
            Token = token
        };
        _logger.LogInformation("END: Register success with username {username}", dto.Username);
        return Ok(resp);
    }
    
}