using Microsoft.EntityFrameworkCore;
using Tutorial12.Model;

namespace Tutorial12.Data;

public class TripDbContext : DbContext
{
    public DbSet<Client> Client { get; set; } = null!;
    public DbSet<Trip> Trip { get; set; } = null!;
    public DbSet<ClientTrip> ClientTrip { get; set; } = null!;
    public DbSet<Country> Country { get; set; } = null!;
    public DbSet<CountryTrip> CountryTrip { get; set; } = null!;

    public TripDbContext(DbContextOptions<TripDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>().ToTable("Client");
        modelBuilder.Entity<Trip>().ToTable("Trip");
        modelBuilder.Entity<Country>().ToTable("Country");
        modelBuilder.Entity<ClientTrip>().ToTable("Client_Trip");
        modelBuilder.Entity<CountryTrip>().ToTable("Country_Trip");
        modelBuilder.Entity<ClientTrip>()
            .HasKey(ct => new { ct.IdClient, ct.IdTrip });
        modelBuilder.Entity<ClientTrip>()
            .HasOne(ct => ct.Client)
            .WithMany(c => c.ClientTrips)
            .HasForeignKey(ct => ct.IdClient);
        modelBuilder.Entity<ClientTrip>()
            .HasOne(ct => ct.Trip)
            .WithMany(t => t.ClientTrips)
            .HasForeignKey(ct => ct.IdTrip);
        modelBuilder.Entity<CountryTrip>()
            .HasKey(ct => new { ct.IdCountry, ct.IdTrip });
        modelBuilder.Entity<CountryTrip>()
            .HasOne(ct => ct.Country)
            .WithMany(c => c.CountryTrips)
            .HasForeignKey(ct => ct.IdCountry);
        modelBuilder.Entity<CountryTrip>()
            .HasOne(ct => ct.Trip)
            .WithMany(t => t.CountryTrips)
            .HasForeignKey(ct => ct.IdTrip);
    }
}