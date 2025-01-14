using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

public class MssqlDbContext : DbContext
{
    public DbSet<Currency> Currencies { get; set; }
    // 建構子接受 DbContextOptions 並傳遞給基底類別

    public override int SaveChanges()
    {
        // SaveChanges時自動更新 Currency.UpdatedAt 欄位
        foreach (var entry in ChangeTracker.Entries<Currency>())
        {
            if (entry.State == EntityState.Modified) // 如果是更新操作
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow; // 設定為當前 UTC 時間
            }
        }

        return base.SaveChanges(); // 呼叫基底 SaveChanges
    }

    public MssqlDbContext(DbContextOptions<MssqlDbContext> options)
        : base(options)
    {
    }
}
