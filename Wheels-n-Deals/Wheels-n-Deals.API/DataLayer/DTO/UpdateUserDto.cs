using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Wheels_n_Deals.API.DataLayer.DTO;

public class UpdateUserDto
{
    [JsonIgnore] public Guid Id { get; set; }
    [MaxLength(50)] public string Email { get; set; } = string.Empty;

    [MaxLength(250)] public string Password { get; set; } = string.Empty;

    [MaxLength(50)] public string FirstName { get; set; } = string.Empty;

    [MaxLength(50)] public string LastName { get; set; } = string.Empty;

    [MaxLength(10)] public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(50)] public string Address { get; set; } = string.Empty;
}
