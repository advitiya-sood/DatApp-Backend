using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DatApp.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Like> GetLike(int userId, int recipientId)
        {
            return await _context.Likes.FirstOrDefaultAsync(u =>
                u.LikerId == userId && u.LikeeId == recipientId);
        }

        public async Task<User> GetUser(int id)
        {
            return await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<User>> GetUsers(string gender, int minAge, int maxAge)
        {
            var users = _context.Users.Include(p => p.Photos).AsQueryable();

            if (!string.IsNullOrEmpty(gender))
            {
                users = users.Where(u => u.Gender == gender);
            }

            if (minAge != 18 || maxAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-maxAge - 1);
                var maxDob = DateTime.Today.AddYears(-minAge);

                users = users.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            }

            return await users.ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

public async Task<IEnumerable<User>> GetUserLikes(string predicate, int userId)
{
    // 1. Start by looking at the Users table
    var users = _context.Users.Include(x => x.Photos).OrderBy(u => u.Username).AsQueryable();
    var likes = _context.Likes.AsQueryable();
//    if (predicate == "Likers")
//     {
//         return await likes.Where(u => u.LikeeId == userId)
//                           .Select(i => i.Liker)
//                           .Include(u => u.Photos) 
//                           .ToListAsync();
//     }

//     if (predicate == "Likees")
//     {
//         return await likes.Where(u => u.LikerId == userId)
//                           .Select(i => i.Likee)
//                           .Include(u => u.Photos) 
//                           .ToListAsync();
//     }


if (predicate == "Likers")
    {
        // "Likers" = People who have liked ME.
        // We filter for users where their "outgoing likes" (Likees) contains my ID.
        return await users.Where(u => u.Likees.Any(like => like.LikeeId == userId))
                          .ToListAsync();
    }

    if (predicate == "Likees")
    {
        // "Likees" = People I have liked.
        // We filter for users where their "incoming likes" (Likers) contains my ID.
        return await users.Where(u => u.Likers.Any(like => like.LikerId == userId))
                          .ToListAsync();
    }

    return await users.ToListAsync();
}



    }
}