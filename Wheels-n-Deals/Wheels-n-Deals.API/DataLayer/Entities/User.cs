using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wheels_n_Deals.API.DataLayer.Enums;

namespace Wheels_n_Deals.API.DataLayer.Entities;

public class User : BaseEntity
{
    [MaxLength(50)] public string Email { get; set; } = string.Empty;

    [MaxLength(250)] public string HashedPassword { get; set; } = string.Empty;

    [MaxLength(50)] public string FirstName { get; set; } = string.Empty;

    [MaxLength(50)] public string LastName { get; set; } = string.Empty;

    [NotMapped] public string FullName => $"{FirstName} {LastName}";

    [MaxLength(10)] public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(50)] public string Address { get; set; } = string.Empty;

    public Role RoleType { get; set; } = Role.User;
}