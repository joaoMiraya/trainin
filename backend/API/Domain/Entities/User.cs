using System.ComponentModel.DataAnnotations;

namespace API.Domain.Entities;
public class User
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string Username { get; set; }
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    [Required]
    public int RoleId { get; set; }
    public Role Role { get; set; } 
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Friendship> SentFriendRequests { get; set; } = new List<Friendship>();
    public ICollection<Friendship> ReceivedFriendRequests { get; set; } = new List<Friendship>();
    public ICollection<Like> Likes { get; set; } = new List<Like>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Reply> Replies { get; set; } = new List<Reply>();
    public ICollection<Report> Reports { get; set; } = new List<Report>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
  
    public User(string username, string email, string firstName, string lastName, Role role, string passwordHash)
    {
        Id = Guid.NewGuid();
        Username = username;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Role = role;
        Password = passwordHash;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    public User() { }
}
