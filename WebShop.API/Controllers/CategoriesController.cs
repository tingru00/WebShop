using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShop.Application.Features.Categories.Commands;
using WebShop.Application.Features.Categories.Queries;

namespace WebShop.API.Controllers;

// Controller för att kunna se vilka kategorier som finns
// Behövs för att kunna lägga till produkter till rätt kategori
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET ALL CATEGORIES-endpoint
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _mediator.Send(new GetAllCategoriesQuery());
        return Ok(categories);
    }

    // CREATE CATEGORY-endpoint
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
    }
}