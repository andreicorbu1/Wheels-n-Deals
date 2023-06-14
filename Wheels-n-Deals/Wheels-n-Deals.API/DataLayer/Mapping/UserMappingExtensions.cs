using Wheels_n_Deals.API.DataLayer.Dtos;
using Wheels_n_Deals.API.DataLayer.Entities;

namespace Wheels_n_Deals.API.DataLayer.Mapping;

public static class UserMappingExtensions
{
    public static UserDto ToUserDto(this User user)
    {
        var userDto = new UserDto()
        {
            Id = user.Id.ToString(),
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            RoleType = user.RoleType.ToString()
        };
        return userDto;
    }
}