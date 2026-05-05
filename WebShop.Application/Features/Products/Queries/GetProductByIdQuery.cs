using MediatR;
using WebShop.Domain.Entities;

namespace WebShop.Application.Features.Products.Queries;

public record GetProductByIdQuery(int Id) : IRequest<Product?>;