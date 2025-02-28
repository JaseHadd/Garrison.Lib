using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using Garrison.Lib.DataAnnotations;
using Garrison.Lib.Extensions;

namespace Garrison.Lib.Models;

public class ApiKey
{
    private static readonly int     s_keyLength = 32;
    private static readonly char[]  s_keyChars = new Range[] { 'A'..'Z', 'a'..'z', '0'..'9' }
        .SelectMany(r => Enumerable
            .Range(r.Start.Value, 1 + r.End.Value - r.Start.Value)
            .Select(c => (char)c))
        .ToArray();
    
    [Required, Key]
    public          uint    Id      { get; set; }

    [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public          DateTime Birth  { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    [Required, FixedLength(32)]
    public          string  Key     { get; init; }

    [Required, MaxLength(64)]
    public          string  Name    { get; set; }

    [Required]
    public          User    Owner       { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public static string GenerateKey()
    {
        byte[] data = new byte[4 * s_keyLength];

        using (var crypto = RandomNumberGenerator.Create())
            crypto.GetBytes(data);
        
        var result = new StringBuilder();

        Enumerable.Range(0, s_keyLength)
            .Select(i => BitConverter.ToUInt32(data, i * 4))
            .Select(i => i % s_keyChars.Length)
            .Select(i => s_keyChars[i])
            .ForEach(i => result.Append(i));

        return result.ToString();
    }
}
