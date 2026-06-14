using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTECommerce.Models.Dtos;

public class ProductDto
{
    public int Id { get; set; }

    public string Name { get; set; }    = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string ImgURL { get; set; } = string.Empty;

    public string SKU { get; set; } = string.Empty; // PROD-001-BLK-M  

    public int Stock { get; set; }

    public DateTime CreationDate {get; set ;} = DateTime.Now;

    public DateTime? UpdateDate {get;set;} = null;

    // Relacion con la tabla Category:
    public int CategoryId { get; set; }

    // Para mostrar la descripcion de la categoria a lq que pertence el producto
    public string CategoryName { get; set; } = string.Empty;

}