
using Serilog;

namespace Cau2_QuanLyNhaTro;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        Application.SetUnhandledExceptionMode(
            UnhandledExceptionMode.CatchException);

        Application.ThreadException += (sender, e) =>
        {
            Log.Error(e.Exception, "Lỗi giao diện chưa xử lý");

            MessageBox.Show(
                "Đã xảy ra lỗi, thông tin đã được ghi lại.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        };

        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            if (e.ExceptionObject is Exception ex)
                Log.Fatal(ex, "Lỗi toàn cục chưa xử lý");
            else
                Log.Fatal("Lỗi toàn cục không xác định");
        };

        try
        {
            ApplicationConfiguration.Initialize();

            string folder = Path.Combine(
                AppContext.BaseDirectory, "Logs");

            Directory.CreateDirectory(folder);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(
                    Path.Combine(folder, "log-.txt"),
                    rollingInterval: RollingInterval.Day,
                    outputTemplate:
                        "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            AppConfig.Load();

            Application.Run(new FormKhachThue());
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Không thể khởi động ứng dụng");

            MessageBox.Show(
                "Không thể khởi động ứng dụng. Vui lòng kiểm tra cấu hình.",
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
