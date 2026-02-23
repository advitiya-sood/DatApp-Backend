using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DatApp.Data;
using DatApp.Dtos;
using DatApp.Models;
using DatApp.Services;

namespace DatApp_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public MessagesController(IMessageService messageService, IMapper mapper)
        {
            _messageService = messageService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessage(int id)
        {

            //validation and exception handleing

            var message = await _messageService.GetMessage(id);

            if (message == null)
                return NotFound();

            return Ok(message);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser([FromQuery] string container)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var messages = await _messageService.GetMessagesForUser(container, userId);

            return Ok(messages);
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int recipientId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var messages = await _messageService.GetMessageThread(userId, recipientId);

            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(MessageForCreationDto messageForCreationDto)
        {
            var senderId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            try
            {
                var message = await _messageService.CreateMessage(messageForCreationDto, senderId);
                return Ok(message);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            try
            {
                await _messageService.DeleteMessage(id, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            try
            {
                await _messageService.MarkMessageAsRead(id, userId);
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}