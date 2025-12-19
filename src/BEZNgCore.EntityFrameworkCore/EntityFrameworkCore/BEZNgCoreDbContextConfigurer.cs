using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace BEZNgCore.EntityFrameworkCore;

public static class BEZNgCoreDbContextConfigurer
{
    public static void Configure(DbContextOptionsBuilder<BEZNgCoreDbContext> builder, string connectionString)
    {
        builder.UseSqlServer(connectionString);
    }

    public static void Configure(DbContextOptionsBuilder<BEZNgCoreDbContext> builder, DbConnection connection)
    {
        builder.UseSqlServer(connection);
    }
}

