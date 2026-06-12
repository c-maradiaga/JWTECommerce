using AutoMapper;
using JWTECommerce.Models.Dtos;
using JWTECommerce.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace JWTECommerce.Controllers;

[ApiController]
[Route("api/[controller]")]
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




}