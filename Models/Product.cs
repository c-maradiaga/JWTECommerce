using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTECommerce.Models;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Descripcion { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public string ImgURL { get; set; } = string.Empty;

    public string SKU { get; set; } = string.Empty; // PROD-001-BLK-M  

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.Now;

    public DateTime? UpdateDate { get; set; } = null;

    // Relacion con la tabla Category:
    [ForeignKey("CategoryId")]
    public int CategoryId { get; set; }

    // Navegacion:
    public required Category Category { get; set; }
}
