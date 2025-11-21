

using BuildingBlocks.Exceptions;

namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid ProductId) : ICommand<Unit>;

    public class DeleteProductHandler(IDocumentSession session) : ICommandHandler<DeleteProductCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var Product = await session.LoadAsync<Product>(request.ProductId, cancellationToken)
                ?? throw new NotFoundException("Product", $"Id: {request.ProductId}");
            session.Delete(Product);
            await session.SaveChangesAsync(cancellationToken);
            return await Task.FromResult(Unit.Value);
        }
    }
}