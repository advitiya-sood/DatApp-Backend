using System.Collections.Generic;
using System.Threading.Tasks;
using DatApp.Models;

namespace DatApp.Data
{
    public interface IUserRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<IEnumerable<User>> GetUsers(string gender, int minAge, int maxAge);
        Task<User> GetUser(int id);
        Task<Like> GetLike(int userId, int recipientId);
    }
}