
using Microsoft.Extensions.Configuration;

namespace Cau3_KeHoachDaNgoai;

public static class AppConfig
{
    public static string ConnectionString { get; private set; } = "";

    public static void Load()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", false, false)
            .Build();

        ConnectionString =
            config.GetConnectionString("DefaultConnection")
            ?? throw new Exception(
                "Chưa cấu hình DefaultConnection.");
    }
}
