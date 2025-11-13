namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand : ICommand<ProductCommandCreateResultDto>
    {
        public string Name { set; get; } = "";
        public string Description { set; get; } = "";
        public List<string> Categories { set; get; } = [];
        public string ImageFile { get; set; } = "";
        public decimal Price { get; set; } = 0;
    }

    public class ProductCommandCreateResultDto
    {
        public Guid Id { set; get; }
        public string Name { set; get; } = "";
        public string Description { set; get; } = "";
        public List<string> Categories { set; get; } = [];
        public string ImageFile { get; set; } = "";
        public decimal Price { get; set; } = 0;
    }

    public class CreateProductHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, ProductCommandCreateResultDto>
    {
        public async Task<ProductCommandCreateResultDto> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var newProduct = command.Adapt<Product>();
            newProduct.Id = Guid.NewGuid();
            session.Store(newProduct);
            await session.SaveChangesAsync(cancellationToken);
            return newProduct.Adapt<ProductCommandCreateResultDto>();
        }
    }
}