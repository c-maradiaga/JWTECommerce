using AutoMapper;
using JWTECommerce.Models;
using JWTECommerce.Models.Dtos;

namespace JWTECommerce.Mapping;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Category, CreateCategoryDto>().ReverseMap();
    }
}