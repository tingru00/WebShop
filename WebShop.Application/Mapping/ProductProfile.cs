using AutoMapper;
using WebShop.Domain.Entities;
using WebShop.Application.DTOs;

namespace WebShop.Application.Mapping;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}