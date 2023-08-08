using Microsoft.EntityFrameworkCore;
using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.DataLayer;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        // set-up delete cascade
        modelBuilder.Entity<User>()
            .HasMany(u => u.Announcements)
            .WithOne(a => a.Owner)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Vehicles)
            .WithOne(v => v.Owner)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Vehicle>()
            .HasOne(v => v.Feature)
            .WithMany(f => f.Vehicles)
            .HasForeignKey(v => v.FeatureId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Vehicle>()
            .HasOne(v => v.Announcement)
            .WithOne(a => a.Vehicle)
            .HasForeignKey<Announcement>(a => a.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AnnouncementImage>()
            .HasKey(ai => new { ai.AnnouncementId, ai.ImageId });
        //modelBuilder.Entity<AnnouncementImage>()
        //    .HasOne(ai => ai.Announcement)
        //    .WithMany(ai => ai.Images)
        //    .HasForeignKey(ai => ai.AnnouncementId);
        //modelBuilder.Entity<AnnouncementImage>()
        //   .HasOne(ai => ai.Image)
        //   .WithMany(ai => ai.Announcements)
        //   .HasForeignKey(ai => ai.ImageId);

        modelBuilder.Entity<Announcement>()
            .HasMany(an => an.Images)
            .WithMany(an => an.Announcements)
            .UsingEntity<AnnouncementImage>();

        // Convert Enums to Strings in Database
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();
        modelBuilder.Entity<Feature>()
            .Property(f => f.Fuel)
            .HasConversion<string>();
        modelBuilder.Entity<Feature>()
            .Property(f => f.Gearbox)
            .HasConversion<string>();
        modelBuilder.Entity<Vehicle>()
            .Property(v => v.TechnicalState)
            .HasConversion<string>();

        // Auto Include 
        modelBuilder.Entity<Vehicle>()
            .Navigation(v => v.Feature)
            .AutoInclude();
        modelBuilder.Entity<Vehicle>()
            .Navigation(v => v.Owner)
            .AutoInclude();

        modelBuilder.Entity<Announcement>()
            .Navigation(an => an.Owner)
            .AutoInclude();
        modelBuilder.Entity<Announcement>()
            .Navigation(an => an.Vehicle)
            .AutoInclude();
        modelBuilder.Entity<Announcement>()
            .Navigation(an => an.Images)
            .AutoInclude();

        modelBuilder.Entity<User>()
            .Navigation(u => u.Vehicles)
            .AutoInclude();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Feature> Features { get; set; }
    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<AnnouncementImage> AnnouncementImages { get; set; }

}
