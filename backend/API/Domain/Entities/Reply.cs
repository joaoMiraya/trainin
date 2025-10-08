
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Domain.Entities;

public class Reply
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int CommentId { get; set; }
    public Comment? Comment { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Report> Reports { get; set; } = new List<Report>();
    public Reply(string content, int commentId, Guid userId)
    {
        Content = content;
        CommentId = commentId;
        UserId = userId;
    }

    public Reply() { }
}
