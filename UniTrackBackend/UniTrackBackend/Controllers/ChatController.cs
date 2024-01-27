﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniTrackBackend.Api.DTO.ResultDtos;
using UniTrackBackend.Services.Chatting;

namespace UniTrackBackend.Controllers
{
    [Route("api/chat")]
    [Authorize]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public ChatController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("people/{userId}")]
        public async Task<IActionResult> GetPeopleToChatWith(string userId)
        {
            try
            {
                var contacts = await _messageService.GetContacts(userId);
                return Ok(contacts);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("messagesInChat")]
        public async Task<IActionResult> GetMessagesInChat(string senderId, string receiverId)
        {
            try
            {
                var messages = await _messageService.GetAllMessagesInChatAsync(senderId, receiverId);
                return Ok(messages);
                
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
