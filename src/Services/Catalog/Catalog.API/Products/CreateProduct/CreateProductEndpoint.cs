
namespace Catalog.API.Products.CreateProduct
{
    public class CreateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder.MapPost("/products", async (ProductCreateRequest product, ISender sender) =>
            {
                var createProductCommand = product.Adapt<CreateProductCommand>();
                var commandCreateResult = await sender.Send(createProductCommand);
                return Results.Created($"/products/{commandCreateResult.Id}", commandCreateResult.Adapt<ProductCreateResponse>());
            })
            .WithName("CreateProduct")
            .Produces<ProductCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create product")
            .WithDescription("");
        }
    }

    public class ProductCreateRequest
    {
        public string Name { set; get; } = "";
        public string Description { set; get; } = "";
        public List<string> Categories { set; get; } = [];
        public string ImageFile { get; set; } = "";
        public decimal Price { get; set; } = 0;
    }

    public class ProductCreateResponse
    {
        public Guid Id { set; get; }
        public string Name { set; get; } = "";
        public string Description { set; get; } = "";
        public List<string> Categories { set; get; } = [];
        public string ImageFile { get; set; } = "";
        public decimal Price { get; set; } = 0;
    }
}