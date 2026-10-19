
namespace Cau4_BanHangTraSua;

public class AppSettings
{
    public ConnectionStringSettings ConnectionStrings { get; set; } = new();
    public PaymentSettings Payment { get; set; } = new();
}

public class ConnectionStringSettings
{
    public string DefaultConnection { get; set; } = "";
}

public class PaymentSettings
{
    public string BankId { get; set; } = "";
    public string AccountNo { get; set; } = "";
    public string AccountName { get; set; } = "";
    public string Template { get; set; } = "compact2";
    public string ShopName { get; set; } = "";
}
