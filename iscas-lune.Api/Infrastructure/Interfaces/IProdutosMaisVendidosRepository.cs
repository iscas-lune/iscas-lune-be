using iscas_lune.Api.Domain.Entities;
using iscaslune.Api.Infrastructure.Interfaces;

namespace iscas_lune.Api.Infrastructure.Interfaces;

public interface IProdutosMaisVendidosRepository : IGenericRepository<ProdutosMaisVendidos>
{
    Task<IList<ProdutosMaisVendidos>> GetProdutosMaisVendidosAsync(IList<Guid> produtosIds);
}
