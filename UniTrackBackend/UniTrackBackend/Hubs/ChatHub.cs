using Microsoft.AspNetCore.SignalR;
using UniTrackBackend.Api.DTO.ResultDtos;

namespace UniTrackBackend.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessageToUser(string receiverUserId, string message)
        {
            var senderUserId = Context.UserIdentifier;
            // Logic to save the message to the database

            // Send the message to the specific user
            await Clients.User(receiverUserId).SendAsync("ReceiveMessage", senderUserId, message);
        }
    }
}
