using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatApp.Data;
using DatApp.Dtos;
using DatApp.Models;

namespace DatApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserForListDto>> GetUsersAsync(string gender, int? minAge, int? maxAge)
        {
            var min = minAge ?? 18;
            var max = maxAge ?? 99;
            
            var users = await _repo.GetUsers(gender, min, max);
            return _mapper.Map<IEnumerable<UserForListDto>>(users);
        }

        public async Task<UserForDetailedDto> GetUserAsync(int id)
        {
            var user = await _repo.GetUser(id);
            if (user == null) return null;
            
            return _mapper.Map<UserForDetailedDto>(user);
        }

        public async Task UpdateUserAsync(int id, UserForUpdateDto userForUpdateDto)
        {
            var user = await _repo.GetUser(id);
            if (user == null) throw new Exception("User not found");

            _mapper.Map(userForUpdateDto, user);

            if (!await _repo.SaveAll())
            {
                throw new Exception($"Updating user {id} failed on save");
            }
        }

        public async Task LikeUserAsync(int currentUserId, int recipientId)
        {
            if (currentUserId == recipientId)
                throw new Exception("You cannot like yourself");

            var like = await _repo.GetLike(currentUserId, recipientId);
            if (like != null)
                throw new Exception("You already liked this user");

            var recipient = await _repo.GetUser(recipientId);
            if (recipient == null)
                throw new Exception("User to like was not found");

            like = new Like
            {
                LikerId = currentUserId,
                LikeeId = recipientId
            };

            _repo.Add<Like>(like);

            if (!await _repo.SaveAll())
                throw new Exception("Failed to like user");
        }
    }
}