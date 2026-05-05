using MediatR;
using WebShop.Domain.Entities;
using WebShop.Domain.Interfaces;

namespace WebShop.Application.Features.Products.Queries;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, List<Product>>
{
    private readonly IProductRepository _repository;

    public GetAllProductsHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync();
    }
}