namespace JWTECommerce.Models.Dtos;

public class UserLoginResponseDto
{
    public UserRegisterDto? UserRegisterDto { get; set; }    //? en este caso el ? es por si No se tuvo un login exitoso
    public string? Token { get; set; }
    public string? Message { get; set; }
}