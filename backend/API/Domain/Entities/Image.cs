
using System.ComponentModel.DataAnnotations;
using API.Shared.Constants;

namespace API.Domain.Entities;

public class Image
{
    public int Id { get; set; }

    [Required]
    public string Url { get; set; } = string.Empty;

    public ImageType Type { get; set; }

    public int PostId { get; set; }
    public Post? Post { get; set; }
    
    public Guid? UserId { get; set; }
    public User? User { get; set; }
}
