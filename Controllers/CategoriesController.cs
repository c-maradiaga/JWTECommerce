using AutoMapper;
using JWTECommerce.Constants;
using JWTECommerce.Models;
using JWTECommerce.Models.Dtos;
using JWTECommerce.Repository.IRepository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace JWTECommerce.Controllers;

[ApiController]
[Route("api/[controller]")]
//! [EnableCors("AllowSpecificOrigin")]
// [EnableCors(PolicyNames.AllowSpecificOrigin)]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    // Se inyectan a traves de su constructor:
    public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    /// [EnableCors("AllowSpecificOrigin")]
    /// [EnableCors(PolicyNames.AllowSpecificOrigin)]
    public IActionResult GetCategories()
    {
        var categories = _categoryRepository.GetCategories();
        var categoriesDto = new List<CategoryDto>();
        foreach (var category in categories)
        {
            categoriesDto.Add(_mapper.Map<CategoryDto>(category));
        }

        return Ok(categoriesDto);
    }

    [HttpGet("{id:int}", Name = "GetCategory")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetCategory(int id)
    {
        var category = _categoryRepository.GetCategory(id);
        if(category == null) return NotFound($"La Categoria con id: {id} no existe");

        var categoryDto = _mapper.Map<CategoryDto>(category);
        return Ok(categoryDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)] 
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
    {
        if(createCategoryDto == null)
            return BadRequest(ModelState);

        if(_categoryRepository.CategoryExists(createCategoryDto.Name))
        {
            ModelState.AddModelError("CustomError","La categoría Ya existe");
            return BadRequest(ModelState);
        }
            
        var category = _mapper.Map<Category>(createCategoryDto);

        if(!_categoryRepository.CreateCategory(category))
        {
            ModelState.AddModelError("CustomError", $"Algo salió mal al guadar la categoria {category.Name}");
            return StatusCode(500, ModelState);
        }
        return CreatedAtRoute("GetCategory", new {id = category.Id}, category);
    }

    [HttpPatch("{id:int}", Name = "UpdateCategory")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateCategory(int id, [FromBody] CreateCategoryDto updateCategoryDto)
    {
        if(!_categoryRepository.CategoryExists(id)) 
            return NotFound($"La categoria con id:{id} no existe");
        if(updateCategoryDto == null)
            return BadRequest(ModelState);

        if(_categoryRepository.CategoryExists(updateCategoryDto.Name))
        {
            ModelState.AddModelError("CustomError", $"La categoria {updateCategoryDto.Name} ya existe");
            return BadRequest(ModelState);
        }

        var category = _mapper.Map<Category>(updateCategoryDto);
        category.Id = id;

        if(!_categoryRepository.UpdateCategory(category))
        {
            ModelState.AddModelError("CustomError", "Algo salio mal al actualizar la categoria");
            return StatusCode(500, ModelState);
        }
        return NoContent();
    }

    [HttpDelete("{id}:int")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteCategory(int id)
    {
        if(!_categoryRepository.CategoryExists(id))
            return NotFound($"La categoria con id: {id} No existe");
        
        var category = _categoryRepository.GetCategory(id);
        if(category == null)
            return NotFound($"La Categoria con Id: {id} No Existe");

        if(!_categoryRepository.DeleteCategory(category))
        {
            ModelState.AddModelError("CustomError", $"Algo salio mal al eliminar la categoria");
            return StatusCode(500, ModelState);
        }
        return NoContent();
    }








}