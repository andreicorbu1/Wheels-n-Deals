using System.ComponentModel.DataAnnotations;

namespace Wheels_n_Deals.API.DataLayer.Dtos;

public class LoginDto
{
    [Required] public string Email { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}