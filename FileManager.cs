using System.Diagnostics.CodeAnalysis;
using Garrison.Lib.Models;

namespace Garrison.Lib;

public interface IFileManager
{
    public enum FileType
    {
        Json, Token, Portrait
    }

    public abstract Task SaveFile(Character @char, FileType type, byte[] data);
    public abstract bool TryGetFile(Character @char, FileType type, [NotNullWhen(true)] out FileInfo? file);

}

public class FileManager(string assetDirectory) : IFileManager
{
    private readonly string _assetDirectory = assetDirectory;

    public Task SaveFile(Character @char, IFileManager.FileType type, byte[] data)
    {
        var file = GetFile(@char, type);
        file.Directory?.Create();

        return File.WriteAllBytesAsync(file.FullName, data);
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