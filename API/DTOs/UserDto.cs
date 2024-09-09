using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class UserDto
{
    [Required]
    public required string Username { get; set; }

    [Required]
    public required string KnownAs { get; set; }

    [Required]
    public required string Token { get; set; }

    [Required]
    public required string Gender { get; set; }

    public string? PhotoUrl { get; set; }

}
