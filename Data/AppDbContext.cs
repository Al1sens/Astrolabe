using Astrolabe.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Astrolabe.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    // Таблица в БД
    public DbSet<StarNode> Stars { get; set; }

    public DbSet<StarConnection> StarConnections { get; set; }

    //Настраиваем два внешних ключа на одну таблицу Stars
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StarConnection>()
            .HasOne(c => c.FromStar)
            .WithMany()
            .HasForeignKey(c => c.FromStarId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StarConnection>()
            .HasOne(c => c.ToStar)
            .WithMany()
            .HasForeignKey(c => c.ToStarId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}
