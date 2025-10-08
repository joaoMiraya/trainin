using API.Shared.Constants;

namespace API.Domain.Entities;
public class Notification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; } = false;
    public NotificationType Type { get; set; }
    public Guid? ReferenceId { get; set; }

    public Notification(Guid userId, string message, NotificationType type, Guid? referenceId = null)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Message = message;
        Type = type;
        ReferenceId = referenceId;
        CreatedAt = DateTime.UtcNow;
    }
    public Notification() { }
}