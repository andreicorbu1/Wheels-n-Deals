using Wheels_n_Deals.API.DataLayer.DTO;
using Wheels_n_Deals.API.DataLayer.Interfaces;
using Wheels_n_Deals.API.DataLayer.Models;
using Wheels_n_Deals.API.Services.Interfaces;

namespace Wheels_n_Deals.API.Services;

public class UserService : IUserService
{
    private IAuthService _authService { get; set; }
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IAuthService authService, IUnitOfWork unitOfWork)
    {
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task<User> DeleteUserAsync(Guid userId)
    {
        var user = await _unitOfWork.Users.GetUserByIdAsync(userId);
        user = await _unitOfWork.Users.RemoveAsync(user.Id);
        await _unitOfWork.SaveChangesAsync();
        return user;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _unitOfWork.Users.GetUserByEmailAsync(email);
    }

    public async Task<User> GetUserByIdAsync(Guid id)
    {
        return await _unitOfWork.Users.GetUserByIdAsync(id);
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _unitOfWork.Users.GetUsersAsync();
    }

    public async Task<string> LoginUserAsync(LoginDto dto)
    {
        var user = await _unitOfWork.Users.GetUserByEmailAsync(dto.Email) ?? throw new Exception($"User with email '{dto.Email}' does not exists!");
        var passwordFine = await Task.Run(() => _authService.VerifyHashedPassword(user.HashedPassword, dto.Password));

        if (passwordFine)
            return await Task.Run(() => _authService.GetToken(user));

        return string.Empty;
    }

    public async Task<Guid> RegisterUserAsync(RegisterDto dto)
    {
        if (dto is null) throw new ArgumentNullException(nameof(dto));

        var existingUserWithEmail = _unitOfWork.Users.Any(u => u.Email == dto.Email);
        if (existingUserWithEmail)
        {
            throw new Exception($"User with email '{dto.Email}' already exists!");
        }
        var hashedPassword = _authService.HashPassword(dto.Password);

        var user = new User
        {
            Email = dto.Email,
            HashedPassword = hashedPassword,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Address = dto.Address,
            PhoneNumber = dto.PhoneNumber,
            Role = DataLayer.Enums.Role.User
        };

        var id = await _unitOfWork.Users.InsertAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return id;
    }

    public async Task<User> UpdateUserAsync(UpdateUserDto dto)
    {
        if (dto is null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrWhiteSpace(dto.Email))
            throw new ArgumentNullException(nameof(dto));
        var user = await GetUserByIdAsync(dto.Id) ?? throw new Exception($"User with email '{dto.Email}' does not exists!");

        user.Address = dto.Address;
        user.PhoneNumber = dto.PhoneNumber;
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Email = dto.Email;
        user.HashedPassword = _authService.HashPassword(dto.Password);
        user.LastModified = DateTime.UtcNow;

        user = await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return user;
    }
}
