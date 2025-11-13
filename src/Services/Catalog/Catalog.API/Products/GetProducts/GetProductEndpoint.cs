using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Products.GetProducts
{
    public class GetProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async (ISender sender) =>
            {
                return await sender.Send(new GetProductsQuery());

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
    }
}