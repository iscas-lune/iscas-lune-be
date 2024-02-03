using iscas_lune.Api.Domain.Entities;
using iscas_lune.Api.Infrastructure.Interfaces;
using iscaslune.Api.Domain.Context;
using iscaslune.Api.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace iscas_lune.Api.Infrastructure.Repositories;

public class ProdutosMaisVendidosRepository(IscasLuneContext context)
    : GenericRepository<ProdutosMaisVendidos>(context), IProdutosMaisVendidosRepository
{
    private readonly IscasLuneContext _context = context;

    public async Task<IList<ProdutosMaisVendidos>> GetProdutosMaisVendidosAsync(IList<Guid> produtosIds)
    {
        return await _context
            .ProdutosMaisVendidos
            .AsNoTracking()
            .Where(x => produtosIds.Contains(x.ProdutoId))
            .ToListAsync();
    }
}
