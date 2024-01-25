using Microsoft.AspNetCore.SignalR;
using UniTrackBackend.Api.DTO.ResultDtos;

namespace UniTrackBackend.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinChat(UserChatResultDto conn)
        {
            await Clients.All
                .SendAsync("RecieveMessage", "admin", $"{conn.FirstName} {conn.LastName} has joined");
        }

        public async Task JoinSpecificChatRoom(UserChatResultDto conn)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conn.ChatRoom);
            await Clients.Group(conn.ChatRoom)
                .SendAsync("RecieveMessage", "admin", $"{conn.LastName} has joined {conn.ChatRoom}");
        
        }
    }
}
