namespace Cau1_SoChiTieuCaNhan
{
    public class AppSettings
    {
        public ConnectionStringSettings ConnectionStrings { get; set; } = new();
    }

    public class ConnectionStringSettings
    {
        public string DefaultConnection { get; set; } = "";
    }
}