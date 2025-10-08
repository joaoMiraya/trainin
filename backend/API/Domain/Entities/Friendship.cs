using API.Shared.Constants;

namespace API.Domain.Entities;

public class Friendship
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid RequesterId { get; set; }
    public User Requester { get; set; }

    public Guid AddresseeId { get; set; }
    public User Addressee { get; set; }

    public FriendshipStatus Status { get; set; }
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

    public Friendship(Guid requesterId, Guid addresseeId, FriendshipStatus status)
    {
        RequesterId = requesterId;
        AddresseeId = addresseeId;
        Status = status;
    }
    public Friendship() { }
}
