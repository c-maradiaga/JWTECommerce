using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JWTECommerce.Models;
using JWTECommerce.Models.Dtos;
using JWTECommerce.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace JWTECommerce.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetProducts()
    {
        var products = _productRepository.GetProducts();
        var productsDto = _mapper.Map<List<ProductDto>>(products);

        // var productsDto = new List<ProductDto>();
        // foreach(var product in products)
        // {
        //     productsDto.Add(_mapper.Map<ProductDto>(product));
        // }

        return Ok(productsDto);
    }

   [HttpGet("{productId:int}", Name = "GetProduct")]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status403Forbidden)]
   [ProducesResponseType(StatusCodes.Status404NotFound)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   [ProducesResponseType(StatusCodes.Status200OK)]
   public IActionResult GetProduct(int productId)
   {
        if(productId <= 0)
            return BadRequest($"Product Id = {productId} no se permite.");

        var product = _productRepository.GetProduct(productId);
        if (product == null)
            return NotFound($"El producto con el id {productId} No Existe");
        
        var productoDto = _mapper.Map<ProductDto>(product);
        return Ok(productoDto);
   }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        if(createProductDto == null)
            return BadRequest(ModelState);

        // Validar la categoria si es valida:
        if(!_categoryRepository.CategoryExists(createProductDto.CategoryId))
        {
            ModelState.AddModelError("CustomError", $"El id de Categoria recibido {createProductDto.CategoryId} No Existe");
            return BadRequest(ModelState);
        }

        if(_productRepository.ProductExists(createProductDto.Name))
        {
            ModelState.AddModelError("CustomError", $"La Producto con nombre {createProductDto.Name} Ya Existe");
            return BadRequest(ModelState);
        }
        
        var product = _mapper.Map<Product>(createProductDto);
        if(!_productRepository.CreateProduct(product))
        {
            ModelState.AddModelError("CustomError", $"Algo salio mal al crear el producto {createProductDto.Name}");
            return StatusCode(500, ModelState);
        }
        //? el productId en el new, debe coincidir con el nombre de productId definido en el metodo GetProduct
        //return CreatedAtRoute("GetProduct", new {productId = product.Id}, product);

        // Para mostrar la descripcion de la categoria en el response, se hace un nuevo mapeo a ProductDto:
        var createdProduct = _productRepository.GetProduct(product.Id);
        var productoDto = _mapper.Map<ProductDto>(createdProduct);

        return CreatedAtRoute("GetProduct", new {productId = product.Id}, productoDto);
    }

    /* Comprar articulo: El Patch es solo porque este verbo se asemeja mas a la accion que se va a hacer,
       pudo ser un Post. Un Get No porque no estamos obteniendo nada.*/
    [HttpPatch("buyProduct/{name}/{quantity:int}", Name ="BuyProduct")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult BuyProduct(string name, int quantity)
    {
        if(string.IsNullOrEmpty(name) || quantity <= 0)
            return BadRequest("El nombre del producto o cantidad no son validos");

        if(!_productRepository.ProductExists(name))
            return BadRequest($"El producto {name} no existe.");

        if(!_productRepository.BuyProduct(name, quantity))
        {
            ModelState.AddModelError("CustomError", $"No se pudo comprar el producto o la cantidad es mayor a la existencia.");
            return BadRequest(ModelState);
        }
        var units = quantity == 1 ? "unidad" : "unidades";
        return Ok($"Se compro {quantity} {units} del producto {name}");
    }

    [HttpPut("{productId:int}", Name = "UpdateProduct")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult UpateProduct(int productId, [FromBody] UpdateProductDto updateProductDto)
    {
        if(updateProductDto == null)
            return BadRequest(ModelState);

        if(!_productRepository.ProductExists(productId))
        {
            return Problem(detail:"El producto No Existe", statusCode:StatusCodes.Status404NotFound, title:"Producto No Existe");
            // ModelState.AddModelError("CustomError", "El producto no existe");
            // return BadRequest(ModelState);
        }

        if(!_categoryRepository.CategoryExists(updateProductDto.CategoryId))
        {
            ModelState.AddModelError("CustomError", $"La categoria {updateProductDto.CategoryId} No existe");
            return BadRequest(ModelState);
        }

        var product = _mapper.Map<Product>(updateProductDto);
        product.Id = productId;

        if(!_productRepository.UpdateProduct(product))
        {
            ModelState.AddModelError("CustomError", "Algo salio mal al actualizar el producto");
            return StatusCode(500, ModelState);
        }
        return NoContent();
    }
}