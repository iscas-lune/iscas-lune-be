using iscas_lune.Api.Application.Interfaces;
using iscas_lune.Api.Dtos.ProdutosMaisVendidos;
using iscas_lune_api.Application.Interfaces;
using iscas_lune_api.Domain.Entities;
using iscas_lune_api.Dtos.Pedidos;
using iscas_lune_api.Exceptions;
using iscas_lune_api.Infrastructure.Interfaces;
using iscas_lune_api.Model.Paginacao;
using iscas_lune_api.Model.Pedidos;

namespace iscas_lune_api.Application.Services;

public class PedidoService(
    IPedidoRepository pedidoRepository,
    ITokenService tokenService,
    IProcessarPedidoService processarPedidoService,
    IProdutosMaisVendidosService produtosMaisVendidosService) : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository = pedidoRepository;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IProcessarPedidoService _processarPedidoService = processarPedidoService;
    private readonly IProdutosMaisVendidosService _produtosMaisVendidosService = produtosMaisVendidosService;

    public async Task<(string? error, bool result)> CreatePedidoAsync(PedidoCreateDto pedidoCreateDto)
    {
        if (pedidoCreateDto.PedidosPorPeso.Count == 0 && pedidoCreateDto.PedidosPorTamanho.Count == 0)
            throw new ExceptionApi("Informe os produtos");

        var claims = _tokenService.GetClaims();
        var date = DateTime.Now;
        var pedido = new Pedido(Guid.NewGuid(), date, date, 0, StatusPedido.Aberto, claims.Id);

        pedido = await _processarPedidoService.ProcessarAsync(pedido, pedidoCreateDto);

        var result = await _pedidoRepository.AddAsync(pedido);

        if (!result) return ("Ocorreu um erro interno, tente novamente mais tarde!", false);

        return (null, true);
    }

    public async Task<PaginacaoViewModel<PedidoViewModel>> GetPaginacaoAsync(PaginacaoPedidoDto paginacaoPedidoDto)
    {
        var paginacao = await _pedidoRepository.GetPaginacaoPedidoAsync(paginacaoPedidoDto);

        return new()
        {
            TotalPage = paginacao.TotalPage,
            Values = paginacao.Values
                .Select(x => new PedidoViewModel().ForModel(x) ?? new())
                .ToList()
        };
    }

    public async Task<List<PedidoViewModelSemItens>> GetPedidosUsuarioAsync(int statusPedido)
    {
        var claims = _tokenService.GetClaims();
        return await _pedidoRepository.GetPedidosByUsuarioIdAsync(claims.Id, statusPedido);
    }

    public async Task<bool> UpdateStatusPedidoAsync(UpdateStatusPedidoDto updateStatusPedidoDto)
    {
        var pedido = await _pedidoRepository.GetPedidoByUpdateStatusAsync(updateStatusPedidoDto.PedidoId)
            ?? throw new ExceptionApi("O pedido não foi localizado!"); ;

        ValidadeUpdateStatus(pedido, updateStatusPedidoDto.StatusPedido);

        pedido.UpdateStatus(updateStatusPedidoDto.StatusPedido);

        await _pedidoRepository.UpdateAsync(pedido);

        if(pedido.StatusPedido == StatusPedido.Entregue)
        {
            var produtosMaisVendidosDto = pedido
                .ItensPedido
                .Select(x =>
                    new AddOrUpdateProdutosMaisVendidosDto()
                    {
                        ProdutoId = x.ProdutoId,
                        QuantidadeProdutos = x.Quantidade
                    })
                .ToList();

            await _produtosMaisVendidosService
                .AddOrUpdateProdutosMaisVendidosAsync(produtosMaisVendidosDto);
        }


        return true;
    }

    private static void ValidadeUpdateStatus(Pedido pedido, StatusPedido statusPedido)
    {
        if (pedido.StatusPedido == StatusPedido.Entregue)
            throw new ExceptionApi("O pedido já foi entregue, e não é possível modificar o status, caso necessário, exclua o pedido!");

        if (pedido.StatusPedido == statusPedido)
            throw new ExceptionApi("Selecione outro status, esse status é o mesmo do pedido!");
    }
}
