using Microsoft.EntityFrameworkCore;
using Wheels_n_Deals.API.DataLayer.Entities;

namespace Wheels_n_Deals.API.DataLayer;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.RoleType)
            .HasConversion<string>();
        modelBuilder.Entity<Features>()
            .Property(feature => feature.FuelType)
            .HasConversion<string>();
        modelBuilder.Entity<Features>()
            .Property(feature => feature.Gearbox)
            .HasConversion<string>();
        modelBuilder.Entity<Vehicle>()
            .Property(vehicle => vehicle.TechnicalState)
            .HasConversion<string>();
        modelBuilder.Entity<Vehicle>()
            .HasIndex(v => v.VinNumber)
            .IsUnique();
        modelBuilder.Entity<Vehicle>()
            .Navigation(v => v.Owner)
            .AutoInclude();
        modelBuilder.Entity<Vehicle>()
            .Navigation(v => v.Features)
            .AutoInclude();
        modelBuilder.Entity<Announcement>()
            .Navigation(a => a.ImagesUrl)
            .AutoInclude();
        modelBuilder.Entity<Announcement>()
            .Navigation(a => a.User)
            .AutoInclude();
        modelBuilder.Entity<Announcement>()
            .Navigation(a => a.Vehicle)
            .AutoInclude();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Features> Features { get; set; }
    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<Image> Images { get; set; }
}