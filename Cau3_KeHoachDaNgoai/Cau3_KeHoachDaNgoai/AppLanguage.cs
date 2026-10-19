
using System.Globalization;
using System.Text.Json;

namespace Cau3_KeHoachDaNgoai;

public static class AppLanguage
{
    public static string Language { get; private set; } = "vi";

    public static bool IsEnglish => Language == "en";

    public static CultureInfo Culture =>
        CultureInfo.GetCultureInfo(
            IsEnglish ? "en-US" : "vi-VN");

    public static string T(string vi, string en)
        => IsEnglish ? en : vi;

    private static string FilePath =>
        Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
            "Cau3_KeHoachDaNgoai",
            "usersettings.json");

    public static void Load()
    {
        try
        {
            if (!File.Exists(FilePath))
                return;

            string json = File.ReadAllText(FilePath);

            var settings =
                JsonSerializer.Deserialize<UserSettings>(json);

            if (settings?.Language is "vi" or "en")
                Language = settings.Language;
        }
        catch (Exception)
        {
            Language = "vi";
        }
    }

    public static void Save(string language)
    {
        if (language != "vi" && language != "en")
            return;

        Language = language;

        string? folder = Path.GetDirectoryName(FilePath);

        if (folder != null)
            Directory.CreateDirectory(folder);

        string json = JsonSerializer.Serialize(
            new UserSettings { Language = language },
            new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(FilePath, json);
    }

    private class UserSettings
    {
        public string Language { get; set; } = "vi";
    }
}
