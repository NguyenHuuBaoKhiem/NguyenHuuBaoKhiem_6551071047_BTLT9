
namespace Cau2_QuanLyNhaTro;

public class AppSettings
{
    public ConnectionStringSettings ConnectionStrings { get; set; } = new();

    public ProvinceApiSettings ProvinceApi { get; set; } = new();
}

public class ConnectionStringSettings
{
    public string DefaultConnection { get; set; } = "";
}

public class ProvinceApiSettings
{
    public string BaseUrl { get; set; } =
        "https://provinces.open-api.vn/api/v2/";

    public int TimeoutSeconds { get; set; } = 10;
}
