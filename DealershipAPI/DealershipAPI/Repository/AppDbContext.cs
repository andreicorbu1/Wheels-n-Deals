using DealershipAPI.Entity;
using Microsoft.EntityFrameworkCore;

namespace DealershipAPI.Repository;

public class AppDbContext : DbContext
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder
			.UseNpgsql("Host=localhost:5432;Database=DealershipWebApp;Username=postgres;Password=root");

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

	}

	public DbSet<User> Users { get; set; }
}