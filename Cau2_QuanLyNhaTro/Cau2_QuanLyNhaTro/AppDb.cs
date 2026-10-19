
using Cau2_QuanLyNhaTro.Models;
using Microsoft.EntityFrameworkCore;

namespace Cau2_QuanLyNhaTro;

public static class AppDb
{
    public static QuanLyNhaTroContext Create()
    {
        var options =
            new DbContextOptionsBuilder<QuanLyNhaTroContext>()
                .UseSqlServer(
                    AppConfig.Settings
                        .ConnectionStrings.DefaultConnection)
                .Options;

        return new QuanLyNhaTroContext(options);
    }
}
