using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebShop.Application.Features.Products.Commands;
using WebShop.Application.Features.Products.Queries;

namespace WebShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // CREATE PRODUCT-endpoint
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductCommand command)
    {
        var id = await _mediator.Send(command);

        return Ok(id);
    }

    // GET ALL PRODUCTS-endpoint 
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _mediator.Send(new GetAllProductsQuery());
        return Ok(products);
    }

    // GET PRODUCT BY ID-endpoint
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    // UPDATE PRODUCT-endpoint
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProductCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        await _mediator.Send(command);

        return NoContent();
    }

    // DELETE PRODUCT-endpoint
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteProductCommand(id));
        return NoContent();
    }
}