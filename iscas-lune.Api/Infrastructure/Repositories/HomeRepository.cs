using iscas_lune.Api.Infrastructure.Interfaces;
using iscas_lune.Api.Model.Home;
using iscaslune.Api.Domain.Context;
using Microsoft.EntityFrameworkCore;

namespace iscas_lune.Api.Infrastructure.Repositories;

public class HomeRepository : IHomeRepository
{
    private readonly IscasLuneContext _context;

    public HomeRepository(IscasLuneContext context)
    {
        _context = context;
    }

    public async Task<HomeViewModel?> GetQuantidadeVendasNoMesAsync()
    {
        var dataMesAnterior = DateTime.Now.AddMonths(-1);

        var ultimasVendasNoMes = await _context
            .Pedidos
            .AsNoTracking()
            .Where(x => x.DataCriacao.Month == DateTime.Now.Month)
            .CountAsync();

        var vendasMesAnterior = await _context
            .Pedidos
            .AsNoTracking()
            .Where(x => x.DataCriacao.Month == dataMesAnterior.Month)
            .CountAsync();

        return new()
        {
            QuantidadeVendasNoMes = ultimasVendasNoMes,
            QuantidadeVendasNoMesAnterior = vendasMesAnterior
        };
    }
}
