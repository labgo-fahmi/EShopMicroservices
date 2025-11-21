using BuildingBlocks.Exceptions;

namespace Catalog.API.Products.GetProductById
{
    public class GetProductByIdQuery : IQuery<GetProductByIdResponse>
    {
        public required Guid ProductId;
    }

    public class GetProductByIdResponse
    {
        public Guid Id { set; get; }
        public string Name { set; get; } = "";
        public string Description { set; get; } = "";
        public List<string> Categories { set; get; } = [];
        public string ImageFile { get; set; } = "";
        public decimal Price { get; set; } = 0;
    }

    public class GetProductByIdHandler(IDocumentSession session) : IQueryHandler<GetProductByIdQuery, GetProductByIdResponse>
    {
        public async Task<GetProductByIdResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(request.ProductId, cancellationToken);
            if (product == null)
            {
                throw new NotFoundException("Product", $"Id: {request.ProductId}");
            }
            return product.Adapt<GetProductByIdResponse>();
        }
    }
}