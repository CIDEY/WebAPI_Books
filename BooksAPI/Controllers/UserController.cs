using BooksAPI.DTO.User;
using BooksAPI.Helpers;
using BooksAPI.Model;
using BooksAPI.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BooksAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var usersDto = users.Select(x => x.ToDto());
            return Ok(usersDto);
        }

        [HttpPut]
        [Authorize(Roles = nameof(UserRole.Administrator))]
        public async Task<IActionResult> UpdateUser (int id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (id != updateUserDto.Id)
                return BadRequest("User ID mismatch.");

            var user = updateUserDto.ToEntity();
            await _userService.UpdateUserAsync(user);

            return NoContent();
        }

        [HttpDelete]
        [Authorize(Roles = nameof(UserRole.Administrator))]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
