using FoodieFlow.GestaoPedido.Core.Entities;
using FoodieFlow.GestaoPedido.Core.Entities.Request;
using FoodieFlow.GestaoPedido.Core.Enum;
using FoodieFlow.GestaoPedido.Core.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace FoodieFlow.GestaoPedido.Routes
{
    public static class PedidosRoutes
    {
        public static void Map(RouteGroupBuilder app)
        {
            app.MapGet("/GetAll", (IPedidoService pedidoService, ILogger<Program> logger, EnumStatus? status) =>
            {
                try
                {
                    logger.LogInformation("Buscando pedidos");
                    var pedidos = pedidoService.GetAll(status);
                    if (pedidos == null || !pedidos.Any())
                    {
                        return Results.NotFound("Pedidos não encontrados");
                    }


                    return Results.Ok(pedidos);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Erro ao buscar pedidos. {ex.Message}");
                    return Results.Problem("Erro ao buscar pedidos", statusCode: 400);
                }
            });

            app.MapPut("/atualizarStatus/{id}", (IPedidoService pedidoService, ILogger<Program> logger, int id, EnumStatus status) =>
            {
                try
                {
                    logger.LogInformation($"Atualizando status do pedido {id} para '{status}'");
                    pedidoService.UpdateStatus(id, status);
                    return Results.Ok("Pedido atualizado com sucesso!");
                }
                catch (Exception ex)
                {
                    logger.LogError($"Erro ao atualizar status do pedido. {ex.Message}");
                    return Results.Problem("Erro ao atualizar status do pedido", statusCode: 400);
                }
            });

            app.MapPost("/processarMensagemSQS", (IProcessamentoService processamentoService, ILogger<Program> logger, MensagemSQS mensagem) =>
            {
                try
                {
                    logger.LogInformation($"Processando mensagem SQS: {mensagem}");

                    processamentoService.ProcessarMensagemSQS(mensagem);

                    return Results.Ok("Mensagem processada com sucesso");
                }
                catch (Exception ex)
                {
                    logger.LogError($"Erro ao processar mensagem SQS. {ex.Message}");
                    return Results.Problem("Erro ao processar mensagem SQS", statusCode: 400);
                }
            });


        }
    }
}
