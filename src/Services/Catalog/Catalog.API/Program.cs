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

var app = builder.Build();

app.MapCarter();
app.UseExceptionHandler();
app.Run();

