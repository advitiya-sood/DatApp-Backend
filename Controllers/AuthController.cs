using System;
using System.Threading.Tasks;
using DatApp.Dtos;
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
            try
            {
                await _authService.Register(userForRegisterDto);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLogin userForLogin)
        {
            var token = await _authService.Login(userForLogin.Username, userForLogin.Password);

            if (token == null)
                return Unauthorized();

            return Ok(new { token });
        }
    }
}