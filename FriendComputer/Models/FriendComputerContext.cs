using Microsoft.EntityFrameworkCore;

namespace FriendComputer.Models
{
  internal class CarlContext : DbContext
  {
    public DbSet<Quote> Quotes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlite("Data Source=db/friend-computer.db");
    }
  }
}
