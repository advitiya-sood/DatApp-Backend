using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatApp.Dtos;

namespace DatApp.Services
{
    public interface IMessageService
    {
        Task<MessageToReturnDto> GetMessage(int id);
        Task<IEnumerable<MessageToReturnDto>> GetMessagesForUser(string container, int userId);
        Task<IEnumerable<MessageToReturnDto>> GetMessageThread(int userId, int recipientId);
        Task<MessageToReturnDto> CreateMessage(MessageForCreationDto messageForCreationDto, int senderId);
        Task DeleteMessage(int id, int userId);
        Task MarkMessageAsRead(int id, int userId);
    }
}