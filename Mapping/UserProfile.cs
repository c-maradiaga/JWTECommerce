using AutoMapper;
using JWTECommerce.Models;
using JWTECommerce.Models.Dtos;

namespace JWTECommerce.Mapping;

public class UserProfile: Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, CreateUserDto>().ReverseMap();
        CreateMap<User, UserLoginDto>().ReverseMap();
        CreateMap<User, UserLoginResponseDto>().ReverseMap();
    }
}