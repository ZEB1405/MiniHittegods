using MiniHittegods.Application;
using Microsoft.EntityFrameworkCore;
using MiniHittegods.Domain.Interfaces;
using MiniHittegods.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();

//register FoundItemService with DI container as scoped lifetime
builder.Services.AddScoped<FoundItemService>();

//Register database context and repository
builder.Services.AddDbContext<FoundItemDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IFoundItemRepository, FoundItemRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

