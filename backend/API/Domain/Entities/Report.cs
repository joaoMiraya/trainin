using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.Shared.Constants;

namespace API.Domain.Entities;

public class Report
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string? Reason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ReportTargetType TargetType { get; set; }
    public int? PostId { get; set; }
    public Post? Post { get; set; }
    public int? CommentId { get; set; }
    public Comment? Comment { get; set; }
    public int? ReplyId { get; set; }
    public Reply? Reply { get; set; }

    public Report(Guid userId, string reason, ReportTargetType targetType, int? postId, int? commentId = null, int? replyId = null)
    {
        UserId = userId;
        Reason = reason;
        TargetType = targetType;
        PostId = postId;
        CommentId = commentId;
        ReplyId = replyId;
    }

    public Report() { }
}

