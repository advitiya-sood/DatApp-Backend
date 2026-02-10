using System.Threading.Tasks;
using DatApp.Dtos;
using DatApp.Models;

namespace DatApp.Services
{
    public interface IAuthService
    {
        Task<User> Register(UserforRegisterdto userForRegisterDto);
        Task<string> Login(string username, string password);
    }
}