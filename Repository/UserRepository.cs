using BCrypt.Net;
using JWTECommerce.Data;
using JWTECommerce.Models;
using JWTECommerce.Models.Dtos;
using JWTECommerce.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JWTECommerce.Repository;

public class UserRepository : IUserRepository
{
    public readonly ApplicationDbContext _db;
    private readonly string? secretKey ;


    public UserRepository(ApplicationDbContext db, IConfiguration configuration)
    {
        _db = db;
        secretKey = configuration.GetValue<string>("ApiSettings:SecretKey");
    }



    public User? GetUser(int id)
    {
        return _db.Users.FirstOrDefault(u => u.Id == id);
    }


    public ICollection<User> GetUsers()
    {
        return _db.Users.OrderBy(u => u.UserName).ToList();
    }


    public bool IsUniqueUser(string username)
    {
        return !_db.Users.Any(u => u.UserName.ToLower().Trim() == username.ToLower().Trim());
    }


    public async Task<UserLoginResponseDto> Login(UserLoginDto userLogingDto)
    {
        if(string.IsNullOrWhiteSpace(userLogingDto.Username))
        {
            return new UserLoginResponseDto()
            {
              Token = "",
              Message = "El Username es requerido"
            };
        }

        var user = await _db.Users.FirstOrDefaultAsync<User>(u => u.UserName.ToLower().Trim() == 
                                                             userLogingDto.Username.ToLower().Trim());
        if(user is null)
        {
            return new UserLoginResponseDto()
            {
                Token = "",
                Message = $"Username: {userLogingDto.Username} No encontrado"
            };
        }

        if(!BCrypt.Net.BCrypt.Verify(userLogingDto.Password, user.Password))
        {
            return new UserLoginResponseDto()
            {
                Token = "",
                Message = "Las credenciales son Incorrectas"
            };
        }




    }

    public async Task<User> Register(CreateUserDto createUserDto)
    {
        var encriptedPassword = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

        var user = new User()
        {
             UserName = createUserDto.Username ?? "No User",
             Name = createUserDto.Name,
             Role = createUserDto.Role,
             Password = encriptedPassword
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return user;

    }
}