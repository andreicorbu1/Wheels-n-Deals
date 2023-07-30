using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Wheels_n_Deals.API.DataLayer.Models;
using Wheels_n_Deals.API.Services.Interfaces;

namespace Wheels_n_Deals.API.Services;

public class AuthService : IAuthService
{
    private readonly string _securityKey;
    private readonly int _PBKDF2IterCount = 10000;
    private readonly int _PBKDF2SubkeyLength = 256 / 8;
    private readonly int _SaltSize = 128 / 8;

    public AuthService(IConfiguration configuration)
    {
        _securityKey = configuration["JWT:SecurityKey"] ?? "u6pCOKWZ3pVBHmNcTToOcHddyB74XkP7";
    }

    public string GetToken(User user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var role = user.Role.ToString();

        var roleClaim = new Claim(ClaimTypes.Role, role);
        var idClaim = new Claim(ClaimTypes.NameIdentifier, user.Id.ToString());
        var infoClaim = new Claim(ClaimTypes.Email, user.Email);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = "Backend",
            Audience = "Frontend",
            Subject = new ClaimsIdentity(new[] { roleClaim, idClaim, infoClaim }),
            Expires = DateTime.Now.AddHours(1),
            SigningCredentials = credentials
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var tokenString = jwtTokenHandler.WriteToken(token);

        return tokenString;
    }

    public bool ValidateToken(string tokenString)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey));

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            IssuerSigningKey = key,
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true
        };

        if (!jwtTokenHandler.CanReadToken(tokenString.Replace("Bearer", string.Empty)))
        {
            Console.WriteLine("Invalid token");
            return false;
        }

        jwtTokenHandler.ValidateToken(tokenString, tokenValidationParameters, out var validatedToken);
        return validatedToken is not null;
    }

    public string HashPassword(string password)
    {
        if (password is null) throw new ArgumentNullException(nameof(password));

        byte[] salt;
        byte[] subkey;
        using (var deriveByte = new Rfc2898DeriveBytes(password, _SaltSize, _PBKDF2IterCount))
        {
            salt = deriveByte.Salt;
            subkey = deriveByte.GetBytes(_PBKDF2SubkeyLength);
        }

        var outputBytes = new byte[1 + _SaltSize + _PBKDF2SubkeyLength];
        Buffer.BlockCopy(salt, 0, outputBytes, 1, _SaltSize);
        Buffer.BlockCopy(subkey, 0, outputBytes, 1 + _SaltSize, _PBKDF2SubkeyLength);
        return Convert.ToBase64String(outputBytes);
    }

    public bool VerifyHashedPassword(string hashedPassword, string password)
    {
        if (hashedPassword is null) return false;
        if (password is null) throw new ArgumentNullException(nameof(password));
        var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);

        if (hashedPasswordBytes.Length != 1 + _SaltSize + _PBKDF2SubkeyLength ||
            hashedPasswordBytes[0] != 0x00) return false;

        var salt = new byte[_SaltSize];
        Buffer.BlockCopy(hashedPasswordBytes, 1, salt, 0, _SaltSize);

        var storedSubkey = new byte[_PBKDF2SubkeyLength];
        Buffer.BlockCopy(hashedPasswordBytes, 1 + _SaltSize, storedSubkey, 0, _PBKDF2SubkeyLength);

        byte[] generatedSubKey;
        using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, _PBKDF2IterCount))
        {
            generatedSubKey = deriveBytes.GetBytes(_PBKDF2SubkeyLength);
        }

        return ByteArraysEqual(storedSubkey, generatedSubKey);
    }

    private static bool ByteArraysEqual(byte[] storedSubkey, byte[] generatedSubKey)
    {
        if (ReferenceEquals(storedSubkey, generatedSubKey)) return true;

        if (storedSubkey is null || generatedSubKey is null || storedSubkey.Length != generatedSubKey.Length)
            return false;

        var areSame = true;
        for (var i = 0; i < storedSubkey.Length; i++) areSame &= storedSubkey[i] == generatedSubKey[i];

        return areSame;
    }
}
