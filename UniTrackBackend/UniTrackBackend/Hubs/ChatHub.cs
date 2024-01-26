using Microsoft.AspNetCore.SignalR;
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
            var senderUserId = Context.UserIdentifier ?? throw new ArgumentNullException("The senderUserId is null");
            // Logic to save the message to the database
            await _messageService.SaveMessageAsync(senderUserId, receiverUserId, content);
            // Send the message to the specific user
            await Clients.User(receiverUserId).SendAsync("ReceiveMessage", senderUserId, content);
        }
    }
}
