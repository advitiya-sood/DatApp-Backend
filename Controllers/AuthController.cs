using System;
using System.Threading.Tasks;
using DatApp.Dtos;
using DatApp.Models;
using DatApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserforRegisterdto userForRegisterDto)
        {
            userForRegisterDto.Email = userForRegisterDto.Email.ToLower();

            if (await _authService.UserExists(userForRegisterDto.Email))
                return BadRequest("Email already exists");

            var createdUser = await _authService.Register(userForRegisterDto);
            return StatusCode(201);
        }

[HttpPost("login")]
public async Task<IActionResult> Login(UserForLogin userForLogin)
{
    var token = await _authService.Login(userForLogin.Email.ToLower(), userForLogin.Password);

    if (token == null)
        return Unauthorized();

    return Ok(new { token });
}

[HttpPost("request-password-reset")]
public async Task<IActionResult> RequestPasswordReset([FromBody] string email)
{
    try
    {
        var user = await _authService.GetUserByEmailAsync(email.ToLower());
        if (user == null)
            return NotFound("User not found");

        var token = Guid.NewGuid().ToString();
        var expiry = DateTime.UtcNow.AddHours(1);

        await _authService.SavePasswordResetTokenAsync(user, token, expiry);

        return Ok(new { token, expiry });
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"An error occurred: {ex.Message}");
    }
}


[HttpPost("reset-password")]
public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
{
    try
    {
        var user = await _authService.GetUserByResetTokenAsync(dto.Token);
        if (user == null)
            return NotFound("Invalid token");

        if (!user.PasswordResetTokenExpiry.HasValue || user.PasswordResetTokenExpiry.Value < DateTime.UtcNow)
            return BadRequest("Token expired");

        await _authService.ResetPasswordAsync(user, dto.NewPassword);

        return Ok("Password reset successful");
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"An error occurred: {ex.Message}");
    }
    }}
}