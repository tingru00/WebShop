using MediatR;
using WebShop.Application.DTOs;

namespace WebShop.Application.Features.Products.Queries;

// Returnerar DTO
public record GetAllProductsQuery() : IRequest<List<ProductDto>>;