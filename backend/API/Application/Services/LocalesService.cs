using System.Text.Json;
using API.Application.DTOs;
using API.Application.Interfaces;

namespace API.Application.Services;

public class LocalesService : ILocalesService
{
    private readonly IWebHostEnvironment _env;

    public LocalesService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public object GetLocales(string fileName)
    {
        var path = Path.Combine(_env.ContentRootPath, "locales", fileName);
        if (!System.IO.File.Exists(path))
            return null;

        var content = System.IO.File.ReadAllText(path);
        var jsonObject = System.Text.Json.JsonSerializer.Deserialize<object>(content);

        return jsonObject;
    }
    public List<object> GetLocalesByLanguage(string language)
    {
        var basePath = Path.Combine(_env.ContentRootPath, "locales", language);

        if (!Directory.Exists(basePath))
        {
            return new List<object>();
        }

        var jsonFiles = Directory.GetFiles(basePath, "*.json", SearchOption.AllDirectories);
        var result = new List<object>();

        foreach (var file in jsonFiles)
        {
            var content = File.ReadAllText(file);

            try
            {
                var jsonObject = JsonSerializer.Deserialize<object>(content);
                result.Add(new
                {
                    FilePath = file.Replace(_env.ContentRootPath + Path.DirectorySeparatorChar, ""),
                    Content = jsonObject
                });
            }
            catch (JsonException)
            {
                // JSON inválido, ignora ou loga
            }
        }

        return result;
    }

}