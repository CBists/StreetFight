using Microsoft.EntityFrameworkCore;

namespace TelegramGame.Data;

public class MainContext : DbContext
{
    public DbSet<Entity.User> Users => Set<Entity.User>();
    public MainContext() => Database.EnsureCreated();
 
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=main.db");
    }
}