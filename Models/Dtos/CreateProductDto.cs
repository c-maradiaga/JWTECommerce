namespace JWTECommerce.Models.Dtos;

public class CreateProductDto
{
    public string Name { get; set; }    = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string ImgURL { get; set; } = string.Empty;

    public string SKU { get; set; } = string.Empty; // PROD-001-BLK-M  

    public int Stock { get; set; }

    public DateTime? UpdateDate {get;set;} = null;

    // Relacion con la tabla Category:
    public int CategoryId { get; set; }

    // Se quito de aqui la navegacion.    
}