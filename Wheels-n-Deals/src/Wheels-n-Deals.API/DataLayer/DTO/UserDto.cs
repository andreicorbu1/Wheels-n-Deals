using System.ComponentModel.DataAnnotations;

namespace Wheels_n_Deals.API.DataLayer.DTO;

public record UserDto(
    [MaxLength(50)] string Id,
    [MaxLength(50)] string FirstName,
    [MaxLength(50)] string LastName,
    [MaxLength(50)] string Email,
    [MaxLength(10)] string PhoneNumber,
    [MaxLength(50)] string Address,
    [MaxLength(10)] string Role,
    DateTime DateCreated,
    DateTime DateModified
);