using AutoMapper;
using JWTECommerce.Models;
using JWTECommerce.Models.Dtos;
using JWTECommerce.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace JWTECommerce.Controllers;

[ApiController]
[Route("api/[controller]")]
// Aplicamos Constructores Primarios de C# para una inyección de dependencias ultra limpia
public class Product2Controller(IProductRepository productRepository, ICategoryRepository categoryRepository, 
                                IMapper mapper) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult GetProducts()
    {
        var products = productRepository.GetProducts();
        var productsDto = mapper.Map<List<ProductDto>>(products);
        
        return Ok(productsDto);
    }

    [HttpGet("{productId:int}", Name = "GetProduct")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetProduct(int productId)
    {
        if (productId <= 0)
        {
            return Problem(
                detail: $"El identificador del producto ({productId}) no tiene un formato válido.",
                statusCode: StatusCodes.Status400BadRequest,
                title: "Parámetro Inválido"
            );
        }

        var product = productRepository.GetProduct(productId);
        if (product is null)
        {
            return Problem(
                detail: $"No se encontró ningún producto registrado con el ID {productId}.",
                statusCode: StatusCodes.Status404NotFound,
                title: "Recurso No Encontrado"
            );
        }
        
        var productoDto = mapper.Map<ProductDto>(product);
        return Ok(productoDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        if (createProductDto is null)
        {
            return Problem(
                detail: "El cuerpo de la solicitud no puede estar vacío.",
                statusCode: StatusCodes.Status400BadRequest,
                title: "Estructura de Datos Inválida"
            );
        }

        if (!categoryRepository.CategoryExists(createProductDto.CategoryId))
        {
            return Problem(
                detail: $"La categoría con ID {createProductDto.CategoryId} especificada no existe.",
                statusCode: StatusCodes.Status400BadRequest,
                title: "Conflicto de Asociación"
            );
        }

        if (productRepository.ProductExists(createProductDto.Name))
        {
            return Problem(
                detail: $"Ya existe un producto registrado con el nombre '{createProductDto.Name}'.",
                statusCode: StatusCodes.Status400BadRequest,
                title: "Registro Duplicado"
            );
        }
        
        var product = mapper.Map<Product>(createProductDto);
        if (!productRepository.CreateProduct(product))
        {
            return Problem(
                detail: $"Ocurrió un error inesperado en el servidor al intentar registrar el producto '{createProductDto.Name}'.",
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Error Interno de Persistencia"
            );
        }

        var createdProduct = productRepository.GetProduct(product.Id);
        var productoDto = mapper.Map<ProductDto>(createdProduct);

        return CreatedAtRoute("GetProduct", new { productId = product.Id }, productoDto);
    }

    [HttpPatch("buyProduct/{name}/{quantity:int}", Name = "BuyProduct")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult BuyProduct(string name, int quantity)
    {
        if (string.IsNullOrEmpty(name) || quantity <= 0)
        {
            return Problem(
                detail: "Los parámetros proporcionados no cumplen con las condiciones mínimas (el nombre es obligatorio y la cantidad debe ser mayor a cero).",
                statusCode: StatusCodes.Status400BadRequest,
                title: "Datos de Entrada Incorrectos"
            );
        }

        if (!productRepository.ProductExists(name))
        {
            return Problem(
                detail: $"El producto solicitado '{name}' no se encuentra en el catálogo.",
                statusCode: StatusCodes.Status400BadRequest,
                title: "Producto Inexistente"
            );
        }

        if (!productRepository.BuyProduct(name, quantity))
        {
            return Problem(
                detail: "La transacción no pudo completarse debido a una falla en el procesamiento o inventario insuficiente para la cantidad solicitada.",
                statusCode: StatusCodes.Status400BadRequest,
                title: "Operación de Compra Fallida"
            );
        }

        var units = quantity == 1 ? "unidad" : "unidades";
        return Ok($"Se compró exitosamente {quantity} {units} del producto {name}.");
    }

    [HttpPut("{productId:int}", Name = "UpdateProduct")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Agregado formalmente para documentar Swagger
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateProduct(int productId, [FromBody] UpdateProductDto updateProductDto)
    {
        if (updateProductDto is null)
        {
            return Problem(
                detail: "Los datos de actualización no pueden ser nulos.",
                statusCode: StatusCodes.Status400BadRequest,
                title: "Estructura Inválida"
            );
        }

        if (!productRepository.ProductExists(productId))
        {
            return Problem(
                detail: $"No es posible actualizar. El producto con ID {productId} no existe.",
                statusCode: StatusCodes.Status404NotFound,
                title: "Producto No Encontrado"
            );
        }

        if (!categoryRepository.CategoryExists(updateProductDto.CategoryId))
        {
            return Problem(
                detail: $"La categoría asignada con ID {updateProductDto.CategoryId} no existe.",
                statusCode: StatusCodes.Status400BadRequest,
                title: "Categoría Inválida"
            );
        }

        var product = mapper.Map<Product>(updateProductDto);
        product.Id = productId;

        if (!productRepository.UpdateProduct(product))
        {
            return Problem(
                detail: "No se pudieron guardar los cambios en la base de datos debido a un inconveniente del sistema.",
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Error de Actualización"
            );
        }

        return NoContent();
    }
}