
namespace Cau3_KeHoachDaNgoai
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            try
            {
                AppLanguage.Load();
                AppConfig.Load();

                Application.Run(new FormChuyenDi());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể khởi động ứng dụng.\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
