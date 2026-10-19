using Serilog;

namespace Cau1_SoChiTieuCaNhan
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            try
            {
                AppConfig.Load();

                string logFolder =
                    Path.Combine(
                        AppContext.BaseDirectory,
                        "Logs");

                Directory.CreateDirectory(logFolder);

                Log.Logger =
                    new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .WriteTo.File(
                            Path.Combine(
                                logFolder,
                                "log-.txt"),
                            rollingInterval:
                                RollingInterval.Day,
                            outputTemplate:
                                "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                        .CreateLogger();

                Application.Run(
                    new Form1());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể khởi động ứng dụng.\n"
                    + ex.Message,
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
}