using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatApp.Data;
using DatApp.Dtos;
using DatApp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepo _repo;
        private readonly IConfiguration _config;

        public AuthService(IAuthRepo repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public async Task<User> Register(UserforRegisterdto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Username))
                throw new Exception("Username already exists");

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };

            return await _repo.Registration(userToCreate, userForRegisterDto.Password);
        }

        public async Task<string> Login(string username, string password)
        {
            // 1. Check if user exists in DB
            var userFromRepo = await _repo.Login(username.ToLower(), password);

            if (userFromRepo == null)
                return null; // Return null if login failed

            // 2. Generate JWT Token (Business Logic)
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
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}