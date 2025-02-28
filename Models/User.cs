using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Garrison.Lib.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Garrison.Lib.Models;

[Index(nameof(UserName), IsUnique = true)]
public class User
{
    [Required]
    public          uint    Id          { get; set; }

    [Required, MaxLength(32), CaseInsensitive]
    public required string  UserName    { get; set; }

    [FixedLength(49)]
    public required byte[]  Password    { get; set; }

    [MaxLength(100)]
    public          string? Email       { get; set; }


    [InverseProperty(nameof(Character.Owner))]
    public List<Character>   Characters  { get; } = [];

    [InverseProperty(nameof(ApiKey.Owner))]
    public List<ApiKey>      ApiKeys     { get; } = [];
}

public class Role
{
    [Required]
    public          uint    Id          { get; set; }

    [Required, MaxLength(32)]
    public required string  Name        { get; set; }
}