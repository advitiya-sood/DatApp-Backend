using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatApp.Data;
using DatApp.Dtos;
using DatApp.Models;

namespace DatApp.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public MessageService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<MessageToReturnDto> GetMessage(int id)
        {
            var message = await _userRepository.GetMessage(id);
            return _mapper.Map<MessageToReturnDto>(message);
        }

        public async Task<IEnumerable<MessageToReturnDto>> GetMessagesForUser(string container, int userId)
        {
            var messages = await _userRepository.GetMessagesForUser(container, userId);
            return _mapper.Map<IEnumerable<MessageToReturnDto>>(messages);
        }

        public async Task<IEnumerable<MessageToReturnDto>> GetMessageThread(int userId, int recipientId)
        {
            var messages = await _userRepository.GetMessageThread(userId, recipientId);
            return _mapper.Map<IEnumerable<MessageToReturnDto>>(messages);
        }

        public async Task<MessageToReturnDto> CreateMessage(MessageForCreationDto messageForCreationDto, int senderId)
        {
            var sender = await _userRepository.GetUser(senderId);
            if (sender == null)
                throw new UnauthorizedAccessException("Sender not found");

            messageForCreationDto.SenderId = senderId;

            var recipient = await _userRepository.GetUser(messageForCreationDto.RecipientId);
            if (recipient == null)
                throw new Exception("Recipient not found");

            var message = _mapper.Map<Message>(messageForCreationDto);
            // Set usernames for display
            message.SenderUsername = sender.Username;
            message.RecipientUsername = recipient.Username;

            _userRepository.Add(message);
    
    
    //try catch


            if (await _userRepository.SaveAll())
                return _mapper.Map<MessageToReturnDto>(message);

            throw new Exception("Creating the message failed on save");
        }

        public async Task DeleteMessage(int id, int userId)
        {
            var message = await _userRepository.GetMessage(id);
            if (message == null)
                throw new Exception("Message not found");

            if (message.SenderId == userId)
                message.SenderDeleted = true;

            if (message.RecipientId == userId)
                message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
                _userRepository.Delete(message);

            if (!await _userRepository.SaveAll())
                throw new Exception("Error deleting the message");
        }

        public async Task MarkMessageAsRead(int id, int userId)
        {
            var message = await _userRepository.GetMessage(id);
            if (message == null)
                throw new Exception("Message not found");

            if (message.RecipientId != userId)
                throw new UnauthorizedAccessException("User not authorized to mark this message as read");

            message.IsRead = true;
            message.DateRead = DateTime.Now;

            if (!await _userRepository.SaveAll())
                throw new Exception("Error marking the message as read");
        }
    }
}