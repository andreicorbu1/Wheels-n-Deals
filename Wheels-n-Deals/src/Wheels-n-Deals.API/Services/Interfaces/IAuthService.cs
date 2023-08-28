using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.Services.Interfaces;

public interface IAuthService
{
    string GetToken(User user);
    bool ValidateToken(string tokenString);
    string HashPassword(string password);
    bool VerifyHashedPassword(string hashedPassword, string password);
}