using DatApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatApp.Data
{
   public interface IAuthRepo
    {

        Task<User> Registration(User user, string password);

        Task<User> Login(string username, string password);

        Task<bool> UserExist(string username);
    }
}
