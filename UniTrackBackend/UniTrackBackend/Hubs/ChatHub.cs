using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using UniTrackBackend.Api.DTO.ResultDtos;
using UniTrackBackend.Services.Chatting;

namespace UniTrackBackend.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        public ChatHub(IMessageService messageService) 
        {
            _messageService = messageService;
        }

        public async Task SendMessageToUser(string receiverUserId, string content)
        {
            var senderUserId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? throw new Exception("Unauthorized");

            // Logic to save the message to the database
            await _messageService.SaveMessageAsync(senderUserId, receiverUserId, content);

            // Send the message to the specific user
            await Clients.User(receiverUserId).SendAsync("ReceiveMessage", senderUserId, content);
        }
    }
}
