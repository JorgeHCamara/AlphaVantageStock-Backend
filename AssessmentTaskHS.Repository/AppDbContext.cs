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

            modelBuilder.Entity<StockQuote>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Symbol)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(x => x.TimeStamp)
                    .IsRequired();

                entity.Property(x => x.OpenPrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(x => x.HighPrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(x => x.LowPrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(x => x.ClosePrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(x => x.Volume)
                    .IsRequired();
            });
        }
    }
}
