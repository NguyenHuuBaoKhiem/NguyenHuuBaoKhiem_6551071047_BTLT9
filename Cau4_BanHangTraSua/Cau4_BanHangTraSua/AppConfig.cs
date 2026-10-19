
using Microsoft.Extensions.Configuration;

namespace Cau4_BanHangTraSua;

public static class AppConfig
{
    public static AppSettings Settings { get; private set; } = new();

    public static void Load()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", false, false)
            .Build();

        Settings = config.Get<AppSettings>()
            ?? throw new Exception("Không đọc được appsettings.json.");

        if (string.IsNullOrWhiteSpace(
            Settings.ConnectionStrings.DefaultConnection))
        {
            throw new Exception("Thiếu DefaultConnection.");
        }
    }
}
