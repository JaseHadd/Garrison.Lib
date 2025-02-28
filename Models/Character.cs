using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Garrison.Lib.DataAnnotations;

namespace Garrison.Lib.Models;

public class Character
{
    public          uint    Id          { get; set; }

    [Required]
    [FixedLength(16), CaseSensitive]
    public required string  FoundryId   { get; set; }

    [MaxLength(64)]
    public          string? Name        { get; set; }

    [Required]
    public required User    Owner       { get; set; }

    [JsonIgnore]
    [MaxLength(1024*1024)]
    [Column(TypeName = "json")]
    public          string? Data        { get; set; }
}