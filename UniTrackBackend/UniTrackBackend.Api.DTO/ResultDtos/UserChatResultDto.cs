using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniTrackBackend.Api.DTO.ResultDtos
{
    public class UserChatResultDto
    {
        public string UserId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string AvatarUrl { get; set; } = null!;
        public string ChatRoom { get; set; } = null!;
    }
}
