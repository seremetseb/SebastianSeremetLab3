using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using TempFukt.Core.Models;

namespace TempFukt.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<Measurement> Measurements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseSqlite("Data Source=TempFuktData.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Measurement>()
                .Property(m => m.Location)
                .HasConversion<string>();
        }
    }
}
