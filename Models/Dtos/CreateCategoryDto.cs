using System.ComponentModel.DataAnnotations;

namespace JWTECommerce.Models.Dtos;

public class CreateCategoryDto
{
    [Required(ErrorMessage = "El nombre de la categoria es requerido")]
    [MaxLength(50, ErrorMessage = "El nombre de la categoria no debe exceder los 50 caracteres")]
    [MinLength(3, ErrorMessage = "El nombre de la categoria debe tener al menos 3 caracteres")]
    public string Name { get; set; } = string.Empty;

}