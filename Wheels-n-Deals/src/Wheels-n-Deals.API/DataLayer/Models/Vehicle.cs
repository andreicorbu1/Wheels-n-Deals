using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Wheels_n_Deals.API.DataLayer.Enums;

namespace Wheels_n_Deals.API.DataLayer.Models;

[Index(nameof(VinNumber), IsUnique = true)]
public class Vehicle
{
    public Guid Id { get; set; }

    [MaxLength(17)] public string VinNumber { get; set; } = string.Empty;

    [MaxLength(50)] public string Make { get; set; } = string.Empty;

    [MaxLength(50)] public string Model { get; set; } = string.Empty;

    public uint Year { get; set; }
    public uint Mileage { get; set; }
    public float PriceInEuro { get; set; }

    [NotMapped] public float PriceInRon => PriceInEuro * 5;

    public State TechnicalState { get; set; }

    [JsonIgnore]
    [ForeignKey(nameof(Feature))]
    public Guid FeatureId { get; set; }


    // Relationships
    public Feature? Feature { get; set; }

    [JsonIgnore] public User? Owner { get; set; }

    [JsonIgnore] public Announcement? Announcement { get; set; }
}