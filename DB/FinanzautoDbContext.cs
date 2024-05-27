using Microsoft.EntityFrameworkCore;

namespace DB
{
    public class FinanzautoDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Sale> Sales { get; set; }

        public FinanzautoDbContext(DbContextOptions<FinanzautoDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Client>().HasIndex(c => c.Email).IsUnique();
        }
    }
}
