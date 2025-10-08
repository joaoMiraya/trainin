namespace API.Domain.Entities;

public class Like
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public int PostId { get; set; }
    public Post? Post { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Like(Guid userId, int postId)
    {
        UserId = userId;
        PostId = postId;
    }

    public Like() { }
}

