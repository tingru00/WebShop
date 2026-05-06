using MediatR;
using Microsoft.EntityFrameworkCore;
using WebShop.Application.Features.Products.Commands;
using WebShop.Application.Mapping;
using FluentValidation;
using WebShop.Application.Common.Behaviors;
using WebShop.Application.Features.Products.Commands;
using WebShop.Domain.Entities;
using WebShop.Domain.Interfaces;
using WebShop.Infrastructure.Data;
using WebShop.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Database connection string och DbContext registreras
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WebShopDb;Trusted_Connection=True;"));

builder.Services.AddScoped<IProductRepository, ProductRepository>();

// MediatR och handlers registreras automatiskt via assembly scanning
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));

builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

// Felhantering
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.StatusCode = 400;
        context.Response.ContentType = "application/json";

        var contextFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();

        if (contextFeature?.Error is FluentValidation.ValidationException ex)
        {
            var errors = ex.Errors.Select(e => e.ErrorMessage);
            await context.Response.WriteAsJsonAsync(errors);
        }
    });
});

app.UseSwagger();
app.UseSwaggerUI();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
