using DatApp.Data;
using DatApp.Dtos;
using DatApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DatApp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IAuthRepo _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepo repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register(UserforRegisterdto userForRegisterdto)

        {
            //validate request

            userForRegisterdto.Username = userForRegisterdto.Username.ToLower();

            if (await _repo.UserExist(userForRegisterdto.Username))
                return BadRequest("username already exist");

            var UserToCreate = new User
            {
                Username = userForRegisterdto.Username
            };
            var createdUser = await _repo.Registration(UserToCreate, userForRegisterdto.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]

        
        public async Task<IActionResult> Login(UserForLogin userForLogin)
        {

                var userFromRepo = await _repo.Login(userForLogin.Username.ToLower(), userForLogin.Password);
                if (userFromRepo == null)
                    return Unauthorized();

                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(2),
                    SigningCredentials = creds
                };

                var TokenHandeler = new JwtSecurityTokenHandler();

                var Token = TokenHandeler.CreateToken(tokenDescriptor);


                return Ok(new
                {
                    Token = TokenHandeler.WriteToken(Token)
                }
                   );

            }

    }
}
