using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DatApp.Dtos;
using DatApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(
            [FromQuery] string gender, 
            [FromQuery] int? minAge, 
            [FromQuery] int? maxAge)
        {
            var users = await _userService.GetUsersAsync(gender, minAge, maxAge);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            try 
            {
                await _userService.UpdateUserAsync(id, userForUpdateDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            try
            {
                await _userService.LikeUserAsync(id, recipientId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

            [HttpGet("likes")]
            public async Task<IActionResult> GetUserLikes([FromQuery] string predicate)
            {
                try
                {
                    if (string.IsNullOrEmpty(predicate))
                        return BadRequest("You must select whether to view 'Likers' or 'Likees'.");

                    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                    if (userIdClaim == null) return Unauthorized();

                    int currentUserId = int.Parse(userIdClaim.Value);
                    
                    var users = await _userService.GetUserLikesAsync(predicate, currentUserId);
                    return Ok(users);
                }
                catch (Exception ex)
                {
                    return BadRequest("Could not retrieve likes: " + ex.Message);
                }
            }
    }
}