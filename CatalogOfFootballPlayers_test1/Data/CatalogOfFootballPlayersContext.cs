using CatalogOfFootballPlayers_test1.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CatalogOfFootballPlayers_test1.Data
{
    public class CatalogOfFootballPlayersContext : DbContext
    {
        public CatalogOfFootballPlayersContext(DbContextOptions<CatalogOfFootballPlayersContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Footballer> Footballers { get; set; } = null!;
        public DbSet<Team> Teams { get; set; } = null!;
    }
}
