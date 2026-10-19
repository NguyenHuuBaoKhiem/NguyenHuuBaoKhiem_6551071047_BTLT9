
using Serilog;

namespace Cau4_BanHangTraSua;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        try
        {
            string logFolder = Path.Combine(
                AppContext.BaseDirectory, "Logs");

            Directory.CreateDirectory(logFolder);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(
                    Path.Combine(logFolder, "log-.txt"),
                    rollingInterval: RollingInterval.Day,
                    outputTemplate:
                        "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] " +
                        "{Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            AppConfig.Load();

            Application.Run(new FormBanHang());
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Ứng dụng gặp lỗi");

            MessageBox.Show(
                "Không thể khởi động ứng dụng.\n" + ex.Message,
                "Lỗi",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
