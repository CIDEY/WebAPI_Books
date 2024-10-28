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
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var token = _tokenService.GenerateJwtToken(user);

            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                Token = token
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