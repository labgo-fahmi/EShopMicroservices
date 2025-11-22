namespace Catalog.API.Products.GetProducts
{
    public record GetProductsRequest(int? PageNumber, int? PageSize);

    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async ([AsParameters] GetProductsRequest query, ISender sender) =>
            {
                return await sender.Send(query.Adapt<GetProductsQuery>());
            })
            .WithName("GetProducts")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
        }
    }
    public class GetProductResponse
    {
        public Guid Id { set; get; }
        public string Name { set; get; } = "";
        public string Description { set; get; } = "";
        public List<string> Categories { set; get; } = [];
        public string ImageFile { get; set; } = "";
        public decimal Price { get; set; } = 0;
    }
    public class GetProductsResponse
    {
        public IEnumerable<GetProductResponse> Products { get; set; } = [];
        public bool HasNextPage { get; set; } = true;
        public bool HasPreviousPage { get; set; } = true;
        public long PageCount { get; set; } = 20L;
        public long TotalCount { get; set; } = 20L;
    }
}