using System.Text.Json;
using GeometryConsole.Models;

namespace GeometryConsole.Services;

public static class SettingsManager
{
    private static readonly string DirectoryPath =
        Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData),
            "GeometryConsole");


    private static readonly string SettingsPath =
        Path.Combine(
            DirectoryPath,
            "settings.json");


    public static void Save(AppSettings settings)
    {
        Directory.CreateDirectory(DirectoryPath);


        string json =
            JsonSerializer.Serialize(
                settings,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });


        File.WriteAllText(
            SettingsPath,
            json);
    }


    public static AppSettings? Load()
    {
        if (!File.Exists(SettingsPath))
        {
            return null;
        }


        string json =
            File.ReadAllText(SettingsPath);


        return JsonSerializer.Deserialize<AppSettings>(json);
    }
}