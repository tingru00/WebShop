using MediatR;

namespace WebShop.Application.Features.Products.Commands;

public record UpdateProductCommand(
    int Id,
    string Name,
    decimal Price,
    int CategoryId
) : IRequest;