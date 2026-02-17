using System;
using System.Threading.Tasks;
using DatApp.Dtos;
using DatApp.Models;

namespace DatApp.Services
{
    public interface IAuthService
    {
        Task<User> Register(UserforRegisterdto userForRegisterDto);
        Task<string> Login(string username, string password);
        Task<bool> UserExists(string email);
        Task<User> GetUserByEmailAsync(string email);
        Task SavePasswordResetTokenAsync(User user, string token, DateTime expiry);
        Task<User> GetUserByResetTokenAsync(string token);
        Task ResetPasswordAsync(User user, string newPassword);
    }
}