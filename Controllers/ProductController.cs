using System.Net.Http.Headers;
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

    // Obtener por Product Id
    [HttpGet("{productId:int}", Name = "GetProduct")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetProduct(int productId)
    {
        if (productId <= 0)
            return BadRequest($"Product Id = {productId} no se permite.");

        var product = _productRepository.GetProduct(productId);
        if (product == null)
            return NotFound($"El producto con el id {productId} No Existe");

        var productoDto = _mapper.Map<ProductDto>(product);
        return Ok(productoDto);
    }

    // Obtener Producto por Category Id:
    [HttpGet("searchByCategoryId/{categoryId:int}", Name = "GetProductsByCategoryId")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetProductsByCategoryId(int categoryId)
    {
        var lstProducts = _productRepository.GetProductForCategory(categoryId);
        if (lstProducts.Count == 0)
            return NotFound($"No existen productos con la categoria {categoryId}");

        var lstProductDto = _mapper.Map<List<ProductDto>>(lstProducts);

        return Ok(lstProductDto);
    }


    // Bucar Producto por Nombre o Descripcion del Producto: el {name:string} No lleva el tipo, solo los tipo int se les puede poner el tipo.
    [HttpGet("searchProductByNameDesription/{nasearchTermme}", Name = "SearchProducts")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetProductForCategory(string searchTerm)
    {
        var lstProducts = _productRepository.SearchProduct(searchTerm);
        if (lstProducts.Count == 0)
            return NotFound($"No existen productos con el nombre o descripción {searchTerm}");

        var lstProductsDto = _mapper.Map<List<ProductDto>>(lstProducts);
        return Ok(lstProductsDto);
    }




    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        if (createProductDto == null)
            return BadRequest(ModelState);

        // Validar la categoria si es valida:
        if (!_categoryRepository.CategoryExists(createProductDto.CategoryId))
        {
            ModelState.AddModelError("CustomError", $"El id de Categoria recibido {createProductDto.CategoryId} No Existe");
            return BadRequest(ModelState);
        }

        if (_productRepository.ProductExists(createProductDto.Name))
        {
            ModelState.AddModelError("CustomError", $"La Producto con nombre {createProductDto.Name} Ya Existe");
            return BadRequest(ModelState);
        }

        var product = _mapper.Map<Product>(createProductDto);
        if (!_productRepository.CreateProduct(product))
        {
            ModelState.AddModelError("CustomError", $"Algo salio mal al crear el producto {createProductDto.Name}");
            return StatusCode(500, ModelState);
        }
        //? el productId en el new, debe coincidir con el nombre de productId definido en el metodo GetProduct
        //return CreatedAtRoute("GetProduct", new {productId = product.Id}, product);

        // Para mostrar la descripcion de la categoria en el response, se hace un nuevo mapeo a ProductDto:
        var createdProduct = _productRepository.GetProduct(product.Id);
        var productoDto = _mapper.Map<ProductDto>(createdProduct);

        return CreatedAtRoute("GetProduct", new { productId = product.Id }, productoDto);
    }





}