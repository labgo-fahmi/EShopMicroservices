using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMarten(opts =>
{
  opts.Connection(builder.Configuration.GetConnectionString("Default")!);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
{
  builder.Services.InitializeMartenWith<CatalogInitialData>();
}
builder.Services.AddCarter(new DependencyContextAssemblyCatalogCustom());
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddMediatR(config =>
{
  config.RegisterServicesFromAssemblyContaining<CreateProductHandler>();
  config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
  config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks()
                .AddNpgSql(builder.Configuration.GetConnectionString("Default")!);

var app = builder.Build();

app.MapCarter();
app.UseExceptionHandler();
app.UseHealthChecks("/health", new HealthCheckOptions()
{
  ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.Run();

