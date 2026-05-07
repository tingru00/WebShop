using MediatR;

namespace WebShop.Application.Features.Categories.Commands;

public class CreateCategoryCommand : IRequest<int>
{
    public string Name { get; set; } = string.Empty;
}