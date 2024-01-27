using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UniTrackBackend.Api.DTO.ResultDtos;
using UniTrackBackend.Data.Commons;
using UniTrackBackend.Data.Database;
using UniTrackBackend.Data.Models;
using UniTrackBackend.Services.Mappings;

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

        public async Task<List<UserResultDto>> GetContacts(string userId)
        {
            var student = await _context.Students.Where(s => s.UserId == userId).FirstOrDefaultAsync();
            if (student != null)
            {
                var usersDtos = await _context.Teachers.Include(t=>t.User)
                    .Where(t=>t.SchoolId == student.SchoolId)
                    .Select(t => new UserResultDto(t.User.Id, t.User.FirstName, t.User.LastName, t.User.Email, t.User.AvatarUrl))
                    .ToListAsync();

                return usersDtos;
                
            }
            var teacher = await _context.Teachers.Where(t => t.UserId == userId).FirstOrDefaultAsync();
            if (teacher != null)
            {
                var userDtos = await _context.Students.Include(s => s.User)
                    .Where(s => s.SchoolId == teacher.SchoolId)
                    .Select(s => new UserResultDto(s.User.Id, s.User.FirstName, s.User.LastName, s.User.Email, s.User.AvatarUrl))
                    .ToListAsync();

                return userDtos;
            }

            throw new Exception("This ID does not belong to a student or a teacher");
        }

        public async Task<List<MessageHistoryResultDto>> GetMessageHistory(string userId)
        {
            try 
            {
                // Fetch messages where the user is either the sender or the receiver
                var messages = await _context.Messages
                    .Include(m => m.Sender)
                    .Include(m => m.Receiver)
                    .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                    .OrderByDescending(m => m.SentAt) // Assuming you have a DateSent field
                    .ToListAsync();

                // Create a dictionary to hold unique chat history
                var uniqueChats = new Dictionary<string, MessageHistoryResultDto>();

                foreach (var message in messages)
                {
                    // Determine the other party in the chat
                    var otherParty = message.SenderId == userId ? message.Receiver : message.Sender;

                    // Check if this chat is already added
                    if (!uniqueChats.ContainsKey(otherParty.Id))
                    {
                        uniqueChats[otherParty.Id] = new MessageHistoryResultDto
                        {
                            FirstName = otherParty.FirstName,
                            LastName = otherParty.LastName,
                            AvatarUrl = otherParty.AvatarUrl,
                            Message = message.Content // You might want to modify this based on your requirements
                        };
                    }
                }

                // Return the unique chat histories
                return uniqueChats.Values.ToList();
            }
            catch(Exception ex)
            {
                throw new Exception("Internal error!",ex);
            }
        }

        public async Task<Message> GetNewMessageAsync(string senderId, string receiverId)
        {
            var message = await _context.Messages.Where(m=>m.SenderId == senderId &&  m.ReceiverId == receiverId).OrderByDescending(m=>m.SentAt).FirstOrDefaultAsync();
            return message == null ? throw new ArgumentNullException("The message was not found") : message;
        }
    }
}
