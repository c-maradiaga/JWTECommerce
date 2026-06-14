
using AutoMapper;
using JWTECommerce.Models;
using JWTECommerce.Models.Dtos;

namespace JWTECommerce.Mapping;

public class ProductProfile: Profile
{
    public ProductProfile()
    {
        // incluyendo la descripcion de la categoria
        CreateMap<Product, ProductDto>()
        .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
        .ReverseMap();

        CreateMap<Product, CreateProductDto>().ReverseMap();
        CreateMap<Product, UpdateProductDto>().ReverseMap();

    }    
}