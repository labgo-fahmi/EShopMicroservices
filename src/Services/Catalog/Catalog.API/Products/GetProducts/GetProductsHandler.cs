namespace Catalog.API.Products.GetProducts
{
    public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 20) : IQuery<GetProductsQueryResponse> { }

    public class GetProductsHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsQueryResponse>
    {
        public async Task<GetProductsQueryResponse> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await session.Query<Product>().ToPagedListAsync(
                request.PageNumber ?? 1, request.PageSize ?? 20, cancellationToken
            );
            return new GetProductsQueryResponse()
            {
                Products = products.ToList().Adapt<List<GetProductQueryResponse>>(),
                HasNextPage = products.HasNextPage,
                HasPreviousPage = products.HasPreviousPage,
                PageCount = products.PageCount,
                TotalCount = products.TotalItemCount,
            };
        }
    }

    public class GetProductQueryResponse
    {
        public Guid Id { set; get; }
        public string Name { set; get; } = "";
        public string Description { set; get; } = "";
        public List<string> Categories { set; get; } = [];
        public string ImageFile { get; set; } = "";
        public decimal Price { get; set; } = 0;
    }

    public class GetProductsQueryResponse
    {
        public IEnumerable<GetProductQueryResponse> Products { get; set; } = [];
        public bool HasNextPage { get; set; } = true;
        public bool HasPreviousPage { get; set; } = true;
        public long PageCount { get; set; } = 20L;
        public long TotalCount { get; set; } = 20L;
    }
}