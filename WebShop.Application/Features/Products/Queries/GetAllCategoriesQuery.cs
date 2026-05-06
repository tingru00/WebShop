using MediatR;
using WebShop.Domain.Entities;

namespace WebShop.Application.Features.Categories.Queries;

public record GetAllCategoriesQuery() : IRequest<List<Category>>;