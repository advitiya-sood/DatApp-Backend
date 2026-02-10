using System.Collections.Generic;
using System.Threading.Tasks;
using DatApp.Dtos;

namespace DatApp.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserForListDto>> GetUsersAsync(string gender, int? minAge, int? maxAge);
        Task<UserForDetailedDto> GetUserAsync(int id);
        Task UpdateUserAsync(int id, UserForUpdateDto userForUpdateDto);
        Task LikeUserAsync(int currentUserId, int recipientId);
    }
}