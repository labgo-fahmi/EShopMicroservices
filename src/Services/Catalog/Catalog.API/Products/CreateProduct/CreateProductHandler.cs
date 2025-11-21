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

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name could not be empty")
                .Length(2, 255).WithMessage("Name length shoud be between 2 and 255 characters");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description could not be empty");
            RuleFor(x => x.Categories).NotEqual([]).WithMessage("You should at least specify one category");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile could not be empty");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
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