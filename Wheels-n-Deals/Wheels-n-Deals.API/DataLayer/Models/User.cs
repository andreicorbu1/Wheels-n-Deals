using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Wheels_n_Deals.API.DataLayer.Enums;

namespace Wheels_n_Deals.API.DataLayer.Models;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    public Guid Id { get; set; }
    [MaxLength(50)]
    public string FirstName { get; set; }
    [MaxLength(50)]
    public string LastName { get; set; }

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";

    [MaxLength(50)]
    public string Email { get; set; }
    [MaxLength(150)]
    public string HashedPassword { get; set; }
    [MaxLength(10)]
    public string PhoneNumber { get; set; }
    [MaxLength(50)]
    public string Address { get; set; }
    public Role Role { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime LastModified { get; set; } = DateTime.UtcNow;


    // Relationships
    [JsonIgnore]
    public ICollection<Vehicle> Vehicles { get; set; }
    [JsonIgnore]
    public ICollection<Announcement> Announcements { get; set; }
}
