using MediatR;

namespace WebShop.Application.Features.Products.Commands;

public record DeleteProductCommand(int Id) : IRequest;