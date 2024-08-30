using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class UserDto
{
    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Token { get; set; }

    public string? PhotoUrl { get; set; }

}
