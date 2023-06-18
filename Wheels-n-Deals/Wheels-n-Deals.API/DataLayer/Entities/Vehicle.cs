using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wheels_n_Deals.API.DataLayer.Enums;

namespace Wheels_n_Deals.API.DataLayer.Entities;

public class Vehicle : BaseEntity
{
    public Guid OwnerId { get; set; }
    [MaxLength(50)] public string VinNumber { get; set; } = string.Empty;
    [MaxLength(50)] public string Make { get; set; } = string.Empty;
    [MaxLength(50)] public string Model { get; set; } = string.Empty;
    public uint Year { get; set; }
    public uint Mileage { get; set; }
    public float PriceInEuro { get; set; }
    [NotMapped] public float PriceInRon => (PriceInEuro * 5);
    public Guid FeatureId { get; set; }
    public State TechnicalState { get; set; }
}
