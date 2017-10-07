using Web.AmazingLadies.Models;
using Microsoft.EntityFrameworkCore;

namespace Web.AmazingLadies.Data
{
    public class ALOContext : DbContext
    {
        public ALOContext(DbContextOptions<ALOContext> options) : base(options)
        {
        }

        public DbSet<LadyModel> Ladies { get; set; }
        public DbSet<OverwatchModel> OverwatchAccounts { get; set; }
        public DbSet<Discord> DiscordAccounts { get; set; }
        public DbSet<BattleTag> BattleNetAccounts { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Modes> Modes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Discord>().ToTable("DiscordAccounts");
            modelBuilder.Entity<Rank>().ToTable("Ranks");
            modelBuilder.Entity<Roles>().ToTable("Roles");
            modelBuilder.Entity<Modes>().ToTable("Modes");
            modelBuilder.Entity<BattleTag>().ToTable("BattleNetAccounts");
            modelBuilder.Entity<OverwatchModel>().ToTable("OverwatchAccounts");
            modelBuilder.Entity<LadyModel>().ToTable("Ladies");
        }

    }
}
