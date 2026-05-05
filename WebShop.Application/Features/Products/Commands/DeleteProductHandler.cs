using MediatR;
using WebShop.Domain.Interfaces;

namespace WebShop.Application.Features.Products.Commands;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _repository;

    public DeleteProductHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id);

        if (product == null)
            throw new Exception("Product not found");

        await _repository.DeleteAsync(product);
    }
}