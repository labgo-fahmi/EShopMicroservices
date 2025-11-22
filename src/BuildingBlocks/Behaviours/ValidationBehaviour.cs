using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;
namespace BuildingBlocks.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validationContext = new ValidationContext<TRequest>(request);
        var validationResult = await Task.WhenAll(validators.Select(v => v.ValidateAsync(validationContext)));
        var validationErrors = validationResult.SelectMany(r => r.Errors);
        if (validationErrors.Any())
        {
            throw new ValidationException(validationErrors);
        }
        return await next(cancellationToken);
    }
}