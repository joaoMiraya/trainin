
namespace API.Domain.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public int PostId { get; set; }
    public Post? Post { get; set; }

    public ICollection<Like> Likes { get; set; } = new List<Like>();
    public ICollection<Reply> Replies { get; set; } = new List<Reply>();
    public ICollection<Report> Reports { get; set; } = new List<Report>();

    public Comment(string text, Guid userId, int postId)
    {
        Text = text;
        UserId = userId;
        PostId = postId;
    }

    public Comment() { }
}
