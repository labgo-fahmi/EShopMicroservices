
using BuildingBlocks.Exceptions;

namespace Catalog.API.Products.UpdateProduct
{
    public class UpdateProductCommand : ICommand<Unit>
    {
        public required Guid Id { set; get; }
        public string Name { set; get; } = "";
        public string Description { set; get; } = "";
        public List<string> Categories { set; get; } = [];
        public string ImageFile { get; set; } = "";
        public decimal Price { get; set; } = 0;
    }

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name could not be empty")
                .Length(2, 255).WithMessage("Name length shoud be between 2 and 255 characters");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description could not be empty");
            RuleFor(x => x.Categories).NotEqual([]).WithMessage("You should at least specify one category");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile could not be empty");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }

    public class UpdateProductHandler(IDocumentSession session) : ICommandHandler<UpdateProductCommand>
    {
        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            if (await session.LoadAsync<Product>(request.Id, cancellationToken) == null)
            {
                throw new NotFoundException("Product", $"Id: {request.Id}");
            }
            session.Update(request.Adapt<Product>());
            await session.SaveChangesAsync(cancellationToken);
            return await Task.FromResult(Unit.Value);
        }
    }
}