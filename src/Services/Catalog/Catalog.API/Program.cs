using Catalog.API.Products.CreateProduct;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
  config.RegisterServicesFromAssemblyContaining<CreateProductHandler>();
});

var app = builder.Build();

app.MapCarter();

app.Run();
