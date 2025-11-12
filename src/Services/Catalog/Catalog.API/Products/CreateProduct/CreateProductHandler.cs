using BuildingBlocks.CQRS;

using Catalog.API.Models;

using Mapster;

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

    public class CreateProductHandler : ICommandHandler<CreateProductCommand, ProductCommandCreateResultDto>
    {
        public Task<ProductCommandCreateResultDto> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var newProduct = command.Adapt<Product>();
            // Save to DB
            return Task.FromResult(newProduct.Adapt<ProductCommandCreateResultDto>());
        }
    }


}