using BuildingBlocks.CQRS;

namespace Catalog.API.Products.GetProductByCategoryEndpoint
{

    public class GetProductByCategoryResult
    {
        public Guid Id { set; get; }
        public string Name { set; get; } = "";
        public string Description { set; get; } = "";
        public List<string> Categories { set; get; } = [];
        public string ImageFile { get; set; } = "";
        public decimal Price { get; set; } = 0;
    }
    public class GetProductsByCategoryResult
    {
        public List<GetProductByCategoryResult> Products { get; set; } = [];
    }
    public class GetProductByCategoryQuery : IQuery<GetProductsByCategoryResult>
    {
        public required string Category { get; set; } = "";
    }

    public class GetProductByCategoryHandler(IDocumentSession session) : IQueryHandler<GetProductByCategoryQuery, GetProductsByCategoryResult>
    {
        public async Task<GetProductsByCategoryResult> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
        {
            var res = await session.Query<Product>().Where(
                p => p.Categories.Any(c => c.Equals(request.Category, StringComparison.CurrentCultureIgnoreCase))).ToListAsync(cancellationToken);
            return new GetProductsByCategoryResult()
            {
                Products = res.Adapt<List<GetProductByCategoryResult>>()
            };
        }
    }
}
