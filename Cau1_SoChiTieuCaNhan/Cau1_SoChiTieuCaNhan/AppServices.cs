using Cau1_SoChiTieuCaNhan.Models;
using Microsoft.EntityFrameworkCore;

namespace Cau1_SoChiTieuCaNhan
{
    public static class AppServices
    {
        public static QL_ChiTieuContext CreateDbContext()
        {
            DbContextOptions<QL_ChiTieuContext> options =
                new DbContextOptionsBuilder<QL_ChiTieuContext>()
                    .UseSqlServer(
                        AppConfig.Settings
                            .ConnectionStrings
                            .DefaultConnection)
                    .Options;

            return new QL_ChiTieuContext(options);
        }
    }
}