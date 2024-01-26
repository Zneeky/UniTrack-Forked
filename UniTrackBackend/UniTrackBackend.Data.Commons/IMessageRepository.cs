using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTrackBackend.Data.Models;

namespace UniTrackBackend.Data.Commons
{
    public interface IMessageRepository: IRepository<Message>
    {
        Task<List<Message>> GetAllMessagesInChatAsync(string senderId, string receiverId);
        Task<Message> GetNewMessageAsync(string senderId, string receiverId);
    }
}
