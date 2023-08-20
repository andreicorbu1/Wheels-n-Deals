using Wheels_n_Deals.API.DataLayer.DTO;
using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.DataLayer.Mapping;

public static class UserMappingExtensions
{
    public static UserDto ToUserDto(this User user)
    {
        var userDto = new UserDto(
            user.Id.ToString(),
            user.FirstName,
            user.LastName,
            user.Email,
            user.PhoneNumber,
            user.Address,
            user.Role.ToString(),
            user.DateCreated,
            user.LastModified
        );
        return userDto;
    }

    public static List<UserDto> ToUserDto(this List<User> users)
    {
        return (from user in users
                let userDto = user.ToUserDto()
                select userDto).ToList();
    }
}