using MediatR;
using WebShop.Domain.Entities;

namespace WebShop.Application.Features.Products.Queries;

public record GetAllProductsQuery() : IRequest<List<Product>>;