using Wheels_n_Deals.API.DataLayer;
using Wheels_n_Deals.API.DataLayer.Dtos;
using Wheels_n_Deals.API.DataLayer.Entities;
using Wheels_n_Deals.API.DataLayer.Enums;
using Wheels_n_Deals.API.Infrastructure.Exceptions;

namespace Wheels_n_Deals.API.Services;

public class UserService
{
    private AuthorizationService AuthService { get; set; }
    private readonly UnitOfWork _unitOfWork;

    public UserService(AuthorizationService authService, UnitOfWork unitOfWork)
    {
        AuthService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> RegisterUser(RegisterDto registerDto)
    {
        if (registerDto is null)
            return Guid.Empty;

        var existingUserWithEmail = _unitOfWork.Users.Any(u => u.Email == registerDto.Email);
        if (existingUserWithEmail)
        {
            throw new ResourceExistingException($"User with email '{registerDto.Email}' already exists!");
        }

        var hashedPassword = AuthService.HashPassword(registerDto.Password);

        var user = new User
        {
            Email = registerDto.Email,
            HashedPassword = hashedPassword,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Address = registerDto.Address,
            PhoneNumber = registerDto.PhoneNumber,
            RoleType = Role.None
        };

        Role roleType;

        if (Enum.TryParse(registerDto.RoleType, true, out roleType)) user.RoleType = roleType;

        var id = await _unitOfWork.Users.Insert(user) ?? Guid.Empty;
        await _unitOfWork.SaveChanges();

        return id;
    }

    public async Task<User> GetUserById(Guid id)
    {
        return await _unitOfWork.Users.GetById(id) ?? new User();
    }

    public async Task<User> UpdateUser(UpdateUserDto updateDto)
    {
        if (updateDto is null || string.IsNullOrEmpty(updateDto.Email))
            return new User();
        var user = await _unitOfWork.Users.GetUserByEmail(updateDto.Email);
      
        if (user is null)
        {
            throw new ResourceMissingException($"User with email '{updateDto.Email}' doesn't exist!");
        }
      
        user.Address = updateDto.Address;
        user.PhoneNumber = updateDto.PhoneNumber;
        user.FirstName = updateDto.FirstName;
        user.LastName = updateDto.LastName;
        user.HashedPassword = AuthService.HashPassword(updateDto.Password);

        var userUpdated = await _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChanges();
        return userUpdated;
    }

    public async Task<string> Validate(LoginDto loginDto)
    {
        var user = await _unitOfWork.Users.GetUserByEmail(loginDto.Email);

        if (user is null)
        {
            throw new ResourceMissingException($"User with email '{loginDto.Email}' doesn't exist!");
        }

        var passwordFine =
            await Task.Run(() => AuthService.VerifyHashedPassword(user.HashedPassword, loginDto.Password));
        if (passwordFine) return await Task.Run(() => AuthService.GetToken(user));

        return string.Empty;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        var users = await _unitOfWork.Users.GetAll();

        return users;
    }
}