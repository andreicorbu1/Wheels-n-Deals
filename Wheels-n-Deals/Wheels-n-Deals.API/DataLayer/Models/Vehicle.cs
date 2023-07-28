using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wheels_n_Deals.API.DataLayer.Enums;

namespace Wheels_n_Deals.API.DataLayer.Models;

[Index(nameof(VinNumber), IsUnique = true)]
public class Vehicle
{
    public Guid Id { get; set; }
    [MaxLength(17)]
    public string VinNumber { get; set; }
    [MaxLength(50)]
    public string Make { get; set; }
    [MaxLength(50)]
    public string Model { get; set; }
    public uint Year { get; set; }
    public uint Mileage { get; set; }
    public float PriceInEuro { get; set; }
    [NotMapped]
    public float PriceInRon => PriceInEuro * 5;
    public State TechnicalState { get; set; }

    [ForeignKey(nameof(Feature))]
    public Guid FeatureId { get; set; }


    // Relationships
    public Feature Feature { get; set; }
    public User Owner { get; set; }
    public Announcement Announcement { get; set; }
}
