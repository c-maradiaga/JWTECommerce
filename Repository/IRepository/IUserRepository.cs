using JWTECommerce.Models;
using JWTECommerce.Models.Dtos;

namespace JWTECommerce.Repository.IRepository;

public interface IUserRepository
{
    ICollection<User> GetUsers();
    User? GetUser(int id);
    bool IsUniqueUser(string username);
    Task<UserLoginResponseDto> Login(UserLoginDto userLogingDto);
    Task<User> Register(CreateUserDto createUserDto);
}