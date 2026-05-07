using MediatR;
using AutoMapper;
using WebShop.Domain.Interfaces;
using WebShop.Application.DTOs;

namespace WebShop.Application.Features.Products.Queries;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public GetAllProductsHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        // Hämtar entities från databasen
        var products = await _repository.GetAllAsync();

        // Mappar om till DTO
        return _mapper.Map<List<ProductDto>>(products);
    }
}