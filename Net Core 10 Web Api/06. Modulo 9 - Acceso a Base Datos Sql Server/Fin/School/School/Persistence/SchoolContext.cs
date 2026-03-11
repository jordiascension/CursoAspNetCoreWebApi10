using Microsoft.EntityFrameworkCore;

using School.Models;

namespace School.Persistence
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options) { }
        public DbSet<Student> Students { get; set; }
        // Add these DbSet properties
        public DbSet<Invoice> Invoices { get; set; } = default!;
        public DbSet<InvoiceLine> InvoiceLines { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Invoice>(b =>
            {
                b.Property(x => x.TaxableBase).HasPrecision(18, 2);
                b.Property(x => x.TotalVat).HasPrecision(18, 2);
                b.Property(x => x.Total).HasPrecision(18, 2);

                // relationship if you use backing field
                b.Navigation(x => x.Lines)
                 .HasField("_lines")
                 .UsePropertyAccessMode(PropertyAccessMode.Field);

                b.HasMany(x => x.Lines)
                 .WithOne(l => l.Invoice)
                 .HasForeignKey(l => l.InvoiceId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<InvoiceLine>(b =>
            {
                // quantities can be 18,3 or 18,4 depending on your business
                b.Property(x => x.Quantity).HasPrecision(18, 3);

                // prices + totals usually 18,2
                b.Property(x => x.UnitPrice).HasPrecision(18, 2);
                b.Property(x => x.LineBase).HasPrecision(18, 2);
                b.Property(x => x.VatAmount).HasPrecision(18, 2);
                b.Property(x => x.LineTotal).HasPrecision(18, 2);

                b.Property(x => x.VatRate).HasConversion<int>();
            });
        }
    }
}
