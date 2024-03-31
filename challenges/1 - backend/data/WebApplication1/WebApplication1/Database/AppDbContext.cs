using Microsoft.EntityFrameworkCore;
using WebApplication1.Database.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<SwitchState> SwitchStates { get; set; }
}