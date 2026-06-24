using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using JWTECommerce.Data;
using JWTECommerce.Models;
using JWTECommerce.Models.Dtos;
using JWTECommerce.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JWTECommerce.Repository;

public class UserRepository : IUserRepository
{
    public readonly ApplicationDbContext _db;
    private readonly string? _secretKey;


    public UserRepository(ApplicationDbContext db, IConfiguration configuration)
    {
        _db = db;
        _secretKey = configuration.GetValue<string>("ApiSettings:SecretKey");
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
        //1. Validando que el username no sea nulo o vacio
        if (string.IsNullOrWhiteSpace(userLogingDto.Username))
        {
            return new UserLoginResponseDto()
            {
                User = null,
                Token = "",
                Message = "El Username es requerido"
            };
        }

        //2. Traer el usuario desde la base de datos y validando que No sea NULL
        var user = await _db.Users.FirstOrDefaultAsync<User>(u => u.UserName.ToLower().Trim() ==
                                                             userLogingDto.Username.ToLower().Trim());
        if (user is null)
        {
            return new UserLoginResponseDto()
            {
                User = null,
                Token = "",
                Message = $"Username: {userLogingDto.Username} No encontrado"
            };
        }

        //3. Validando el password proporcionado contra el guarado en la BD
        if (!BCrypt.Net.BCrypt.Verify(userLogingDto.Password, user.Password))
        {
            return new UserLoginResponseDto()
            {
                User = null,
                Token = "",
                Message = "Las credenciales son Incorrectas"
            };
        }

        //? 4. Una vez validadas las credenciales se genera el JWT:
        var handlerToken = new JwtSecurityTokenHandler();

        if (string.IsNullOrWhiteSpace(_secretKey))
        {
            throw new InvalidOperationException("Secret Key No Esta configurada");
        }

        var key = Encoding.UTF8.GetBytes(_secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
           [
           new Claim("id" , user.Id.ToString()),
           new Claim("username",  user.UserName),
           new Claim(ClaimTypes.Role, user.Role ??  string.Empty)
           ]),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        //? 5. Creando la respuesta pa
        var token = handlerToken.CreateToken(tokenDescriptor);
        return new UserLoginResponseDto()
        {
            Token = handlerToken.WriteToken(token),
            User = new UserRegisterDto()
            {
                UserName = user.UserName,
                Name = user.Name,
                Role = user.Role,
                Password = user.Password ?? ""
            },
            Message = "Login Exitoso"
        };
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