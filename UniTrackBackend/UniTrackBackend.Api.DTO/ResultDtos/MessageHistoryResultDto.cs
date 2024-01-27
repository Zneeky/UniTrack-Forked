using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniTrackBackend.Api.DTO.ResultDtos
{
    public class MessageHistoryResultDto
    {
        public string ReceiverUserId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string AvatarUrl { get; set; } = null!;
        public string Message { get; set; } = null!;

    }
}
