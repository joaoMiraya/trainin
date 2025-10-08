using System.ComponentModel.DataAnnotations;

namespace API.Application.DTOs;

public class PostDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public Guid UserId { get; set; }

    public string? UserName { get; set; }

    public List<string> ImageUrls { get; set; } = new();

    public int CommentCount { get; set; }

    public int LikeCount { get; set; }
}

public class CreatePostDto
{
    [Required]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 100 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Title can only contain alphanumeric characters and spaces.")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "Content must be between 10 and 500 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9\s.,!?]+$", ErrorMessage = "Content can only contain alphanumeric characters, spaces, and basic punctuation (.,!?).")]
    [DataType(DataType.MultilineText)]
    public string Content { get; set; } = string.Empty;

    public List<string>? ImageUrls { get; set; }
}

public class UpdatePostDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 100 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Title can only contain alphanumeric characters and spaces.")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "Content must be between 10 and 500 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9\s.,!?]+$", ErrorMessage = "Content can only contain alphanumeric characters, spaces, and basic punctuation (.,!?).")]
    [DataType(DataType.MultilineText)]
    public string Content { get; set; } = string.Empty;

    public List<string>? NewImageUrls { get; set; }
}

public class DeletePostDto
{
    [Required]
    public int Id { get; set; }
}