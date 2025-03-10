using System.Diagnostics.CodeAnalysis;
using Garrison.Lib.Models;
using Garrison.Lib.Utilities;
using ImageMagick;

namespace Garrison.Lib;

public static class IFileManagerExtensions
{
    private static ulong s_KB = (ulong)Math.Pow(2, 10);
    private static ulong s_MB = (ulong)Math.Pow(2, 20);
    private static ulong s_GB = (ulong)Math.Pow(2, 30);

    public static string GetMimeType(this IFileManager.FileType type) => type switch
    {
        IFileManager.FileType.Json => "application/json",
        IFileManager.FileType.Token | IFileManager.FileType.Portrait => "image/webp",
        _ => throw new NotImplementedException()
    };

    public static FileSize GetSizeLimit(this IFileManager.FileType type) => (FileSize)(type switch
    {
        IFileManager.FileType.Json => 0.5 * s_MB,
        IFileManager.FileType.Token => 2 * s_MB,
        IFileManager.FileType.Portrait => 5 * s_MB,
        _ => throw new NotImplementedException()
    });
}

public interface IFileManager
{
    public enum FileType
    {
        Json, Token, Portrait
    }

    public abstract Task SaveFile(Character @char, FileType type, Stream data, long? lengthHint = 0);
    public abstract Task SaveFile(Character @char, FileType type, byte[] data);
    public abstract bool TryGetFile(Character @char, FileType type, [NotNullWhen(true)] out FileInfo? file);
}

public abstract class FileManagerException(string message) : Exception(message) {}
public class TooLargeException(FileSize size, FileSize limit) : FileManagerException($"File too large: {size} > {limit}");

public class FileManager(string assetDirectory) : IFileManager
{
    private readonly string _assetDirectory = assetDirectory;


    public async Task SaveFile(Character @char, IFileManager.FileType type, Stream data, long? lengthHint = 0)
    {
        long length;
        if (lengthHint is long l)
            length = l;
        else
            length = data.Length;

        if (length > type.GetSizeLimit())
            throw new TooLargeException((FileSize)length, type.GetSizeLimit());

        byte[] bytes = new byte[length];
        data.ReadExactly(bytes);
        await SaveFile(@char, type, bytes);
    }

    public async Task SaveFile(Character @char, IFileManager.FileType type, byte[] data)
    {
        var file = GetFile(@char, type);
        file.Directory?.Create();

        if (type.GetSizeLimit() < data.Length)

        if (type is IFileManager.FileType.Token or IFileManager.FileType.Portrait)
        {
            MagickImage image = new(data, MagickFormat.Unknown);
            MagickGeometry size = type switch
            {
                IFileManager.FileType.Token => new(400, 400) { FillArea = true },
                IFileManager.FileType.Portrait => new() { Width = 1024 },
                _ => throw new NotImplementedException()
            };

            image.Format = MagickFormat.WebP;
            image.Resize(size);

            data = image.ToByteArray();
        }

        await File.WriteAllBytesAsync(file.FullName, data);
    }

    public bool TryGetFile(Character @char, IFileManager.FileType type, [NotNullWhen(true)] out FileInfo? file)
    {
        return (file = GetFile(@char, type)).Exists;
    }

    private FileInfo GetFile(Character @char, IFileManager.FileType type)
    {
        var path = $"{_assetDirectory}/characters/{@char.FoundryId}";
        var fileName = type switch
        {
            IFileManager.FileType.Json => "data.json",
            IFileManager.FileType.Token => "token.webp",
            IFileManager.FileType.Portrait => "portrait.webp",
            _ => throw new NotImplementedException()
        };

        return new($"{path}/{fileName}");
    }
}