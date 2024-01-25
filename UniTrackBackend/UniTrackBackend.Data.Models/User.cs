using Microsoft.AspNetCore.Identity;

namespace UniTrackBackend.Data.Models;

public class User : IdentityUser
{
    public User() 
    {
        SentMessages = new List<Message>();
        ReceivedMessages = new List<Message>();
    }
    public required string FirstName { get; set; } = null!;

    public required string LastName { get; set; } = null!;

    public string? RefreshToken { get; set; } = null!;
    
    public DateTime? RefreshTokenValidity { get; set; }

    public string AvatarUrl { get; set; } = null!;
    public int SchoolId { get; set; }

    public ICollection<Message> SentMessages { get; set; }
    public ICollection<Message> ReceivedMessages { get; set; }
}