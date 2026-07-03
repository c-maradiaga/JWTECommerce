namespace JWTECommerce.Models.Dtos;

public class UserRegisterDto
{
    public string? Id { get; set; }   //? para este Dto el Id es string
    public string? Name { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public string? Role { get; set; }  
}