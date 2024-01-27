using UniTrackBackend.Api.DTO.ResultDtos;

namespace UniTrackBackend.Services.Chatting
{
    public interface IMessageService
    {
        Task<List<MessageResultDto>> GetAllMessagesInChatAsync(string senderId, string receiverId);
        Task<MessageResultDto> GetNewMessage(string senderId, string receiverId);
        Task SaveMessageAsync(string senderId, string receiverId, string content);
        Task<List<UserResultDto>> GetContacts(string userId);
        Task<List<MessageHistoryResultDto>> GetMessageHistory(string userId);
    }
}