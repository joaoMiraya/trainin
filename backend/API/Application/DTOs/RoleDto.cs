
namespace API.Application.DTOs;

public class RoleDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class RoleWithUsersDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<BasicUserDTO> Users { get; set; } = new();
}