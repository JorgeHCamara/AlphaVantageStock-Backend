using AssessmentTaskHS.Domain.Stocks;
using Microsoft.EntityFrameworkCore;

namespace AssessmentTaskHS.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<StockQuote> StockQuote { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StockQuote>().HasKey(p => p.Id);
        }
    }
}
