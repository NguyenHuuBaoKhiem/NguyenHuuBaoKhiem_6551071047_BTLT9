
using Microsoft.Extensions.Configuration;

namespace Cau2_QuanLyNhaTro;

public static class AppConfig
{
    public static AppSettings Settings { get; private set; } = new();

    public static void Load()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile(
                "appsettings.json",
                optional: false,
                reloadOnChange: true);

#if DEBUG
        builder.AddJsonFile(
            "appsettings.Development.json",
            optional: true,
            reloadOnChange: true);
#endif

        var configuration = builder.Build();

        Settings = configuration.Get<AppSettings>()
                   ?? new AppSettings();

        if (string.IsNullOrWhiteSpace(
            Settings.ConnectionStrings.DefaultConnection))
        {
            throw new Exception(
                "Chưa cấu hình DefaultConnection trong appsettings.json");
        }
    }
}
