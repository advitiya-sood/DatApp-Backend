using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatApp.Data;
using DatApp.Dtos;
using DatApp.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DatApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UsersController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(
            [FromQuery] string gender,
            [FromQuery] int? minAge,
            [FromQuery] int? maxAge)
        {
            var users = _context.Users
                .Include(u => u.Photos)
                .AsQueryable();

            // Apply gender filter if provided
            if (!string.IsNullOrEmpty(gender))
            {
                users = users.Where(u => u.Gender == gender);
            }

            // Apply age filter if provided
            if (minAge.HasValue || maxAge.HasValue)
            {
                var today = DateTime.UtcNow.Date;
                var max = maxAge ?? 99;
                var min = minAge ?? 18;

                var minDob = today.AddYears(-max - 1);
                var maxDob = today.AddYears(-min);

                users = users.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            }

            var userList = await users.ToListAsync();

            var result = _mapper.Map<IEnumerable<UserForListDto>>(userList);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<UserForDetailedDto>(user);

            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != id)
            {
                return Unauthorized();
            }

            var userFromRepo = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (userFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(userForUpdateDto, userFromRepo);

            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            throw new Exception($"Updating user {id} failed on save");
        }

        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (currentUserId != id)
            {
                return Unauthorized();
            }

            if (id == recipientId)
            {
                return BadRequest("You cannot like yourself");
            }

            var like = await _context.Likes.FindAsync(id, recipientId);

            if (like != null)
            {
                return BadRequest("You already liked this user");
            }

            var recipient = await _context.Users.FirstOrDefaultAsync(u => u.Id == recipientId);

            if (recipient == null)
            {
                return NotFound("User to like was not found");
            }

            like = new Like
            {
                LikerId = id,
                LikeeId = recipientId
            };

            await _context.Likes.AddAsync(like);

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }

            throw new Exception("Failed to like user");
        }
    }
}

