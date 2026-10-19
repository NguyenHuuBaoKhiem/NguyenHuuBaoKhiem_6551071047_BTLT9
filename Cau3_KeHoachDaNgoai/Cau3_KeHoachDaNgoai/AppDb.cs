
using Cau3_KeHoachDaNgoai.Models;
using Microsoft.EntityFrameworkCore;

namespace Cau3_KeHoachDaNgoai;

public static class AppDb
{
    public static QuanLyDaNgoaiContext Create()
    {
        var options =
            new DbContextOptionsBuilder<QuanLyDaNgoaiContext>()
                .UseSqlServer(AppConfig.ConnectionString)
                .Options;

        return new QuanLyDaNgoaiContext(options);
    }
}
