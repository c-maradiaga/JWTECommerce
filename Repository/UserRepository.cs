using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTECommerce.Models;
using JWTECommerce.Models.Dtos;
using JWTECommerce.Repository.IRepository;

namespace JWTECommerce.Repository;

public class UserRepository : IUserRepository
{
    public User? GetUser(int id)
    {
        throw new NotImplementedException();
    }

    public ICollection<User> GetUsers()
    {
        throw new NotImplementedException();
    }

    public bool IsUniqueUser(string username)
    {
        throw new NotImplementedException();
    }

    public Task<UserLoginResponseDto> Login(UserLoginDto userLogingDto)
    {
        throw new NotImplementedException();
    }

    public Task<User> Register(CreateUserDto createUserDto)
    {
        throw new NotImplementedException();
    }
}