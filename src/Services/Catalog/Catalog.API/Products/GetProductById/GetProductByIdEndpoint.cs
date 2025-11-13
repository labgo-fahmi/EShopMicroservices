using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Products.GetProductById
{
    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{productId}", (ISender sender, Guid productId) =>
            {
                return sender.Send(new GetProductByIdQuery() { ProductId = productId });
            })
            .WithName("GetProduct")
            .Produces<GetProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError); ;
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
}