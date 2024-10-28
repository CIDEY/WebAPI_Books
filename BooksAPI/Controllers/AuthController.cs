using BooksAPI.DTO.User;
using BooksAPI.Model;
using BooksAPI.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BooksAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
        {
            var user = await _userService.Authenticate(loginDto.Username, loginDto.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var token = _tokenService.GenerateJwtToken(user);

            return Ok(new
            {
                access_token = token,
                token_type = "Bearer",
                expires_in = 3600 // время жизни токена в секундах
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                Username = registerUserDto.Username,
                Role = UserRole.Administrator
            };

            var createdUser = await _userService.Create(user, registerUserDto.Password);
            return Ok(new
            {
                message = "Registration successful",
                UserId = createdUser.Id
            });
        }
    }
}