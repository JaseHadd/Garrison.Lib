using System.ComponentModel.DataAnnotations;
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
}