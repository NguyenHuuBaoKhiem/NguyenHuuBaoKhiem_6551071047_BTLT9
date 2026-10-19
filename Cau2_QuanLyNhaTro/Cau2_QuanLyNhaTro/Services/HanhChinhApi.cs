
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cau2_QuanLyNhaTro.Services;

public class DonViHanhChinh
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("wards")]
    public List<DonViHanhChinh>? Wards { get; set; }

    public override string ToString() => Name;
}

public static class HanhChinhApi
{
    private static readonly HttpClient Client = TaoClient();

    private static HttpClient TaoClient()
    {
        return new HttpClient
        {
            BaseAddress = new Uri(
                AppConfig.Settings.ProvinceApi.BaseUrl),

            Timeout = TimeSpan.FromSeconds(
                AppConfig.Settings.ProvinceApi.TimeoutSeconds)
        };
    }

    public static async Task<List<DonViHanhChinh>>
        LayTinhThanhAsync(CancellationToken token = default)
    {
        string json = await Client.GetStringAsync("", token);

        return JsonSerializer.Deserialize<List<DonViHanhChinh>>(json)
               ?? new List<DonViHanhChinh>();
    }

    public static async Task<List<DonViHanhChinh>>
        LayPhuongXaAsync(int maTinh, CancellationToken token = default)
    {
        string json = await Client.GetStringAsync(
            $"p/{maTinh}?depth=2", token);

        var tinh = JsonSerializer.Deserialize<DonViHanhChinh>(json);

        return tinh?.Wards ?? new List<DonViHanhChinh>();
    }
}
