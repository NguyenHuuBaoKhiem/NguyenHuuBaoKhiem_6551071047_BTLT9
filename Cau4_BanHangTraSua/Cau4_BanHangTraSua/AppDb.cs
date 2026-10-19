
using Cau4_BanHangTraSua.Models;
using Microsoft.EntityFrameworkCore;

namespace Cau4_BanHangTraSua;

public static class AppDb
{
    public static QLTraSuaContext Create()
    {
        var options = new DbContextOptionsBuilder<QLTraSuaContext>()
            .UseSqlServer(AppConfig.Settings.ConnectionStrings.DefaultConnection)
            .Options;

        return new QLTraSuaContext(options);
    }
}
