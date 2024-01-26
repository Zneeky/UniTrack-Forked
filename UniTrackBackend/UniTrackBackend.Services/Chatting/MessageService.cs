using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTrackBackend.Api.DTO.ResultDtos;
using UniTrackBackend.Data.Commons;
using UniTrackBackend.Data.Models;
using UniTrackBackend.Services.Mappings;

namespace UniTrackBackend.Services.Chatting
{
    public class MessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task SaveMessageAsync(string senderId, string receiverId, string content)
        {
            try
            {
                var message = new Message
                {
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    Content = content,
                    SentAt = DateTime.Now,
                    IsRead = false
                };
                await _unitOfWork.MessageRepository.AddAsync(message);
                await _unitOfWork.SaveAsync();
            }
            catch(Exception e)
            {
                throw new Exception("The massage could not be saved", e);
            }

        }

        public async Task<List<MessageResultDto>> GetAllMessagesInChatAsync(string senderId, string receiverId)
        {
            try
            {
                var messages = await _unitOfWork.MessageRepository.GetAllMessagesInChatAsync(senderId, receiverId);
                var messageDtos = new List<MessageResultDto>();
                foreach (var message in messages)
                {
                    var dto = _mapper.MapMessageResultDto(message) ?? throw new Exception("Null message dto");
                    messageDtos.Add(dto);
                }
                return messageDtos;
            }
            catch (Exception e)
            {
                throw new Exception("The massage dtos could not be loaded", e);
            }
        }

        public async Task<MessageResultDto> GetNewMessage(string senderId, string receiverId)
        {
            try
            {
                var message = await _unitOfWork.MessageRepository.GetNewMessageAsync(senderId, receiverId);
                var messageDto = _mapper.MapMessageResultDto(message) ?? throw new Exception("Null message dto");
                return messageDto;
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while ", e);
            }
        }


    }
}
