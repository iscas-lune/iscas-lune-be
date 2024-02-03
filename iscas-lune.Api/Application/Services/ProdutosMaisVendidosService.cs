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
            .Select(x => x.ProdutoId)
            .ToList();

        var produtosMaisVendidos = await _repository.GetProdutosMaisVendidosAsync(produtosIds);

        var add = new List<ProdutosMaisVendidos>();
        var updates = new List<ProdutosMaisVendidos>();
        var date = DateTime.Now;

        foreach (var addOrUpdateDto in addOrUpdateProdutosMaisVendidosDtos)
        {
            var addOrUpdate = produtosMaisVendidos
                .FirstOrDefault(x => x.ProdutoId == addOrUpdateDto.ProdutoId);

            if (addOrUpdate == null)
            {
                var insert = add.FirstOrDefault(x => x.ProdutoId == addOrUpdateDto.ProdutoId);

                if (insert == null)
                {
                    addOrUpdate = new ProdutosMaisVendidos(
                        Guid.NewGuid(),
                        date,
                        date,
                        0,
                        addOrUpdateDto.QuantidadeProdutos,
                        1,
                        addOrUpdateDto.ProdutoId);

                    add.Add(addOrUpdate);
                }
                else
                {
                    insert.UpdateQuantidadeProdutos(addOrUpdateDto.QuantidadeProdutos);
                }

            }
            else
            {
                var update = updates.FirstOrDefault(x => x.ProdutoId == addOrUpdateDto.ProdutoId);

                if(update == null)
                {
                    addOrUpdate.UpdateQuantidadeProdutos(addOrUpdateDto.QuantidadeProdutos);
                    updates.Add(addOrUpdate);
                }
                else
                {
                    update.UpdateQuantidadeProdutos(addOrUpdateDto.QuantidadeProdutos);
                }

            }
        }

        await _repository.AddRangeAsync(add);
        await _repository.UpdateRangeAsync(updates);
    }
}
