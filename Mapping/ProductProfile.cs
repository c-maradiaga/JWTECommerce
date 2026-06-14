
using AutoMapper;
using JWTECommerce.Models;
using JWTECommerce.Models.Dtos;

namespace JWTECommerce.Mapping;

public class ProductProfile: Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Product, CreateProductDto>().ReverseMap();
        CreateMap<Product, UpdateProductDto>().ReverseMap();

    }    
}