using System.Net.Mime;

namespace Yomu.Api.Services;

public class ImageService
{
    private const string directory = "images";
    private readonly Dictionary<string, string> mimeTypes = new Dictionary<string, string>()
    {
        {".gif", "image/gif"},
        {".png", "image/png"},
        {".jpg", "image/jpg"},
        {".jpeg", "image/jpg"}
    };
    private long currentId;

    public ImageService()
    {
        var dir = Directory.CreateDirectory(directory);
        var files = dir.GetFiles();
        currentId = files.Any() ? files.Select(x =>
            {
                if (Int64.TryParse(Path.GetFileNameWithoutExtension(x.Name), out var id))
                {
                    id = 0;
                }
                return id;
            })
            .Max() + 1 : 0;
    }

    public async Task<string?> SaveImage(Stream source, string type)
    {
        string extension = mimeTypes.FirstOrDefault(x => x.Value == type).Key;
        if (string.IsNullOrEmpty(extension))
        {
            return null;
        }

        var filename = $"{Interlocked.Increment(ref currentId)}{extension}";
        using var fs = new FileStream(Path.Combine(directory, filename), FileMode.CreateNew);
        await source.CopyToAsync(fs);

        return filename;
    }

    public async Task<string?> LoadImage(string filename, MemoryStream target)
    {
        var path = Path.Combine(directory, filename);
        if (!File.Exists(path))
        {
            return null;
        }
        
        using var fs = new FileStream(path, FileMode.Open);
        await fs.CopyToAsync(target);

        var extension = Path.GetExtension(filename);
        if (!mimeTypes.TryGetValue(extension, out var mime))
        {
            return null;
        }
        return mime;
    }
}
