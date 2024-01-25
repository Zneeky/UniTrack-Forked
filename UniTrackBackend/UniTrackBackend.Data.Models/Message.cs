using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniTrackBackend.Data.Models
{
    public class Message
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Sender))]
        public string SenderId { get; set; } = null!;
        public User Sender { get; set; } = null!;

        [ForeignKey(nameof(Receiver))]
        public string ReceiverId { get; set; } = null!;
        public User Receiver { get; set; } = null!;

        public string Content { get; set; } = null!;
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }

    }
}
