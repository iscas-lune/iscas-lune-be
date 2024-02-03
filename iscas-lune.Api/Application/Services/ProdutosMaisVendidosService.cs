using iscas_lune.Api.Application.Interfaces;
using iscas_lune.Api.Domain.Entities;
using iscas_lune.Api.Dtos.ProdutosMaisVendidos;
using iscas_lune.Api.Infrastructure.Interfaces;

namespace iscas_lune.Api.Application.Services;

public class ProdutosMaisVendidosService(IProdutosMaisVendidosRepository repository)
    : IProdutosMaisVendidosService
{
    private readonly IProdutosMaisVendidosRepository _repository = repository;

    public async Task AddOrUpdateProdutosMaisVendidosAsync(
        IList<AddOrUpdateProdutosMaisVendidosDto> addOrUpdateProdutosMaisVendidosDtos)
    {
        var produtosIds = addOrUpdateProdutosMaisVendidosDtos
            .Select(x => x.ProdutoId).ToList();

        var produtosMaisVendidos = await _repository.GetProdutosMaisVendidosAsync(produtosIds);

        var listUpdates = produtosMaisVendidos
            .Where(x => 
                addOrUpdateProdutosMaisVendidosDtos.Any(p => p.ProdutoId == x.ProdutoId))
            .ToList();

        var listAdd = addOrUpdateProdutosMaisVendidosDtos
            .Where(x => 
                !produtosMaisVendidos.Any(p => p.ProdutoId == x.ProdutoId))
            .ToList();

        foreach (var item in listUpdates)
        {
            var quantidadesProdutos = addOrUpdateProdutosMaisVendidosDtos
                .FirstOrDefault(x => x.ProdutoId == item.ProdutoId)?.QuantidadeProdutos ?? 0;

            item.UpdateQuantidadeProdutos(quantidadesProdutos);
        }

        var date = DateTime.Now;
        var add = listAdd.Select(x => new ProdutosMaisVendidos(
            Guid.NewGuid(),
            date,
            date,
            0,
            x.QuantidadeProdutos,
            1,
            x.ProdutoId))
            .ToList();

        await _repository.AddRangeAsync(add);
        await _repository.UpdateRangeAsync(listUpdates);
    }
}
