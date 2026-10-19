
using System.Globalization;

namespace Cau4_BanHangTraSua.Services;

public static class VietQrService
{
    private static readonly HttpClient Client = new()
    {
        Timeout = TimeSpan.FromSeconds(10)
    };

    public static string TaoNoiDungChuyenKhoan()
    {
        return "TRASUA DH" +
            DateTime.Now.ToString("MMddHHmmss") +
            Random.Shared.Next(100, 1000);
    }

    public static string TaoDuongDan(
        PaymentSettings payment,
        decimal soTien,
        string noiDung)
    {
        string bank = Uri.EscapeDataString(payment.BankId);
        string account = Uri.EscapeDataString(payment.AccountNo);
        string template = Uri.EscapeDataString(payment.Template);

        string amount = soTien.ToString("0", CultureInfo.InvariantCulture);
        string info = Uri.EscapeDataString(noiDung);
        string name = Uri.EscapeDataString(payment.AccountName);

        return $"https://img.vietqr.io/image/{bank}-{account}-{template}.png" +
               $"?amount={amount}&addInfo={info}&accountName={name}";
    }

    public static Task<byte[]> TaiAnhAsync(string url)
    {
        return Client.GetByteArrayAsync(url);
    }
}
