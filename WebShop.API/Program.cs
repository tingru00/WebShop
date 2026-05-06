using MediatR;
using Microsoft.AspNetCore.Identity;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings.GetValue<string>("Key")!);

// Autentisering med JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // vem som skapat token
        ValidateAudience = true, // mottagare
        ValidateLifetime = true, // kontrollera att token inte gått ut
        ValidateIssuerSigningKey = true, // kontrollera signering

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// Authorization
builder.Services.AddAuthorization();

// Services
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

//Database connection string och DbContext registreras
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WebShopDb;Trusted_Connection=True;"));

builder.Services.AddScoped<IProductRepository, ProductRepository>();

// MediatR och handlers registreras automatiskt via assembly scanning
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));

// Validering med FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Identity, hanterar users, lösenord, roller
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Skapar roller i databasen om de inte finns
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

app.UseAuthentication();
app.UseAuthorization();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Skapar databasen om den inte finns 
    db.Database.Migrate();

    if (!db.Categories.Any())
    {
        db.Categories.AddRange(
            new Category { Name = "Electronics" },
            new Category { Name = "Clothing" },
            new Category { Name = "Books" }
        );

        db.SaveChanges();
    }
}

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