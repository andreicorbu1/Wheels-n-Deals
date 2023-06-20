using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wheels_n_Deals.API.DataLayer.Dtos;

public class UserDto
{
    [MaxLength(50)] public string Id { get; set; } = string.Empty;

    [MaxLength(50)] public string Email { get; set; } = string.Empty;

    [NotMapped] public string FullName { get; set; } = string.Empty;

    [MaxLength(10)] public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(50)] public string Address { get; set; } = string.Empty;

    public string RoleType { get; set; } = string.Empty;
}