using System.ComponentModel.DataAnnotations;

namespace Wheels_n_Deals.API.DataLayer.DTO;

public record RegisterDto(
    [MaxLength(50)] string Email,
    [MaxLength(250)] string Password,
    [MaxLength(50)] string FirstName,
    [MaxLength(50)] string LastName,
    [MaxLength(10)] string PhoneNumber,
    [MaxLength(50)] string Address
);