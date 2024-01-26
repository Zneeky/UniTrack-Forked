using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UniTrackBackend.Data.Commons;
using UniTrackBackend.Data.Database;
using UniTrackBackend.Data.Models;

namespace UniTrackBackend.Data.Repositories
{
    public class MessageRepository : EfRepository<Message>, IMessageRepository
    {
        private readonly UniTrackDbContext _context;
        public MessageRepository(UniTrackDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Message>> GetAllMessagesInChatAsync(string senderId, string receiverId)
        {
            var messagesInChat = await _context.Messages.Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) || (m.SenderId == receiverId && m.ReceiverId == senderId)).ToListAsync();
            return messagesInChat;
        }

        public async Task<Message> GetNewMessageAsync(string senderId, string receiverId)
        {
            var message = await _context.Messages.Where(m=>m.SenderId == senderId &&  m.ReceiverId == receiverId).OrderByDescending(m=>m.SentAt).FirstOrDefaultAsync();
            return message == null ? throw new ArgumentNullException("The message was not found") : message;
        }
    }
}
