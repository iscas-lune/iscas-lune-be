using iscas_lune.Api.Dtos.ProdutosMaisVendidos;

namespace iscas_lune.Api.Application.Interfaces;

public interface IProdutosMaisVendidosService
{
    Task AddOrUpdateProdutosMaisVendidosAsync(IList<AddOrUpdateProdutosMaisVendidosDto> addOrUpdateProdutosMaisVendidosDtos);
}
