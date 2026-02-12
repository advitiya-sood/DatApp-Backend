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
        Task<Like> GetLike(int userId, int recipientId); // check if user is already liked or not
        Task<IEnumerable<User>> GetUserLikes(string predicate, int userId); // get list of users that liked or are liked by the user
    
    
        Task<Message> GetMessage(int id);
        Task<IEnumerable<Message>> GetMessagesForUser(string container, int userId); // Inbox/Outbox/Unread
        Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId); // Chat history
    
    
    }



}