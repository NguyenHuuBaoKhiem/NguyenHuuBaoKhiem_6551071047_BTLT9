
using System.Globalization;
using System.Text.Json;

namespace Cau3_KeHoachDaNgoai.Services;

public class KhongTimThayDiaDanhException : Exception
{
}

public class DuBaoThoiTiet
{
    public DateOnly Ngay { get; set; }
    public int WeatherCode { get; set; }
    public double? NhietDoMin { get; set; }
    public double? NhietDoMax { get; set; }
    public int? XacSuatMua { get; set; }
}

public static class ThoiTietService
{
    private static readonly HttpClient Client = new()
    {
        Timeout = TimeSpan.FromSeconds(8)
    };

    public static async Task<DuBaoThoiTiet?> LayDuBaoAsync(
        string diaDiem,
        DateOnly ngayDi)
    {
        DateOnly homNay = DateOnly.FromDateTime(DateTime.Today);

        if (ngayDi < homNay ||
            ngayDi >= homNay.AddDays(16))
        {
            return null;
        }

        // Bước 1: Tìm tọa độ địa điểm
        string geoUrl =
            "https://geocoding-api.open-meteo.com/v1/search" +
            "?name=" + Uri.EscapeDataString(diaDiem) +
            "&count=1&language=vi";

        using var geoResponse = await Client.GetAsync(geoUrl);
        geoResponse.EnsureSuccessStatusCode();

        string geoJson = await geoResponse.Content.ReadAsStringAsync();

        using JsonDocument geoDoc = JsonDocument.Parse(geoJson);

        if (!geoDoc.RootElement.TryGetProperty(
                "results", out JsonElement results) ||
            results.ValueKind != JsonValueKind.Array ||
            results.GetArrayLength() == 0)
        {
            throw new KhongTimThayDiaDanhException();
        }

        JsonElement location = results[0];

        double latitude =
            location.GetProperty("latitude").GetDouble();

        double longitude =
            location.GetProperty("longitude").GetDouble();

        // Bước 2: Lấy thời tiết 16 ngày
        string lat = latitude.ToString(
            CultureInfo.InvariantCulture);

        string lon = longitude.ToString(
            CultureInfo.InvariantCulture);

        string forecastUrl =
            "https://api.open-meteo.com/v1/forecast" +
            "?latitude=" + lat +
            "&longitude=" + lon +
            "&daily=weather_code,temperature_2m_max," +
            "temperature_2m_min,precipitation_probability_max" +
            "&timezone=auto&forecast_days=16";

        using var response = await Client.GetAsync(forecastUrl);
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();

        using JsonDocument doc = JsonDocument.Parse(json);

        JsonElement daily =
            doc.RootElement.GetProperty("daily");

        JsonElement days = daily.GetProperty("time");
        JsonElement codes = daily.GetProperty("weather_code");
        JsonElement minTemps =
            daily.GetProperty("temperature_2m_min");
        JsonElement maxTemps =
            daily.GetProperty("temperature_2m_max");
        JsonElement rain =
            daily.GetProperty("precipitation_probability_max");

        // Tìm đúng vị trí của ngày đi
        string ngayCanTim = ngayDi.ToString(
            "yyyy-MM-dd", CultureInfo.InvariantCulture);

        int index = -1;

        for (int i = 0; i < days.GetArrayLength(); i++)
        {
            if (days[i].GetString() == ngayCanTim)
            {
                index = i;
                break;
            }
        }

        if (index == -1)
            return null;

        return new DuBaoThoiTiet
        {
            Ngay = ngayDi,
            WeatherCode = codes[index].GetInt32(),
            NhietDoMin = LaySo(minTemps[index]),
            NhietDoMax = LaySo(maxTemps[index]),
            XacSuatMua = LayPhanTram(rain[index])
        };
    }

    private static double? LaySo(JsonElement value)
    {
        return value.ValueKind == JsonValueKind.Number
            ? value.GetDouble()
            : null;
    }

    private static int? LayPhanTram(JsonElement value)
    {
        return value.ValueKind == JsonValueKind.Number
            ? (int)Math.Round(value.GetDouble())
            : null;
    }

    public static string LayBieuTuong(int code)
    {
        if (code == 0) return "☀️";
        if (code >= 1 && code <= 3) return "☁️";
        if (code == 45 || code == 48) return "🌫️";
        if (code >= 95 && code <= 99) return "⛈️";

        if ((code >= 51 && code <= 67) ||
            (code >= 80 && code <= 82))
        {
            return "🌧️";
        }

        return "🌤️";
    }

    public static string LayMoTa(int code)
    {
        if (code == 0)
            return AppLanguage.T("Trời quang", "Clear sky");

        if (code >= 1 && code <= 3)
            return AppLanguage.T("Có mây", "Cloudy");

        if (code == 45 || code == 48)
            return AppLanguage.T("Sương mù", "Foggy");

        if (code >= 95 && code <= 99)
            return AppLanguage.T("Dông", "Thunderstorm");

        if ((code >= 51 && code <= 67) ||
            (code >= 80 && code <= 82))
        {
            return AppLanguage.T("Mưa", "Rain");
        }

        if (code >= 71 && code <= 77)
            return AppLanguage.T("Tuyết", "Snow");

        if (code >= 85 && code <= 86)
            return AppLanguage.T("Tuyết rào", "Snow showers");

        return AppLanguage.T(
            "Thời tiết thay đổi", "Variable weather");
    }
}
