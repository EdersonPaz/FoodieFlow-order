using FoodieFlow.GestaoPedido.Config;
using FoodieFlow.GestaoPedido.Core;
using FoodieFlow.GestaoPedido.Infra;
using FoodieFlow.GestaoPedido.Routes;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterDependencies(builder, configuration);
builder.Services.RegisterCoreModule();
builder.Services.RegisterInfrastructureModule();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/healthcheck", (ILogger<Program> _logger) =>
{
    _logger.LogInformation("Exemplo de endpoint sem autenticação/autorização");

    return Results.Ok();
});

RouteGroupBuilder pedidoItems = app.MapGroup("Pedido/")
    .WithTags("Gestão dos pedidos")
    .WithOpenApi();
PedidosRoutes.Map(pedidoItems);

app.Run();


