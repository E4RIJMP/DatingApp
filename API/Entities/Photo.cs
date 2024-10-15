using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Photos")]
public class Photo
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public bool IsMain { get; set; }
    public string? PublicId { get; set; }
    public bool IsApproved { get; set; } = false;

    // Navitation properties
    public int AppUserId { get; set; } // Required foreign key property
    public AppUser AppUser { get; set; } = null!; // Required reference navigation to principal

    /**
    * Further reading on navitation properties above:
    * https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-many#required-one-to-many
    * https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-forgiving
    **/
}