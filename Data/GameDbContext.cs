using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TicTacToeApi.Models;

namespace TicTacToeApi.Data;

public class GameDbContext : DbContext
{
    public GameDbContext(DbContextOptions<GameDbContext> options) : base(options)
    {
    }

    public DbSet<Game> Games => Set<Game>();
    public DbSet<Move> Moves => Set<Move>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>()
            .HasIndex(g => g.Code)
            .IsUnique();

        modelBuilder.Entity<Game>()
            .HasMany(g => g.Moves)
            .WithOne(m => m.Game!)
            .HasForeignKey(m => m.GameId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}