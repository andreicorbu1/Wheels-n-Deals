using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Wheels_n_Deals.API.DataLayer.Enums;

namespace Wheels_n_Deals.API.DataLayer.Models;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    public Guid Id { get; set; }

    [MaxLength(50)] public string FirstName { get; set; } = string.Empty;

    [MaxLength(50)] public string LastName { get; set; } = string.Empty;

    [NotMapped] public string FullName => $"{FirstName} {LastName}";

    [MaxLength(50)] public string Email { get; set; } = string.Empty;

    [MaxLength(150)] public string HashedPassword { get; set; } = string.Empty;

    [MaxLength(10)] public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(50)] public string Address { get; set; } = string.Empty;

    public Role Role { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime LastModified { get; set; } = DateTime.UtcNow;


    // Relationships
    [JsonIgnore] public List<Vehicle> Vehicles { get; set; } = new();

    [JsonIgnore] public List<Announcement> Announcements { get; set; } = new();
}