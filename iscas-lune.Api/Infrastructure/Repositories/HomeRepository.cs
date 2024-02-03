using iscas_lune.Api.Domain.Entities;
using iscas_lune.Api.Infrastructure.Interfaces;
using iscas_lune.Api.Model.Home;
using iscaslune.Api.Domain.Context;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace iscas_lune.Api.Infrastructure.Repositories;

public class HomeRepository(IscasLuneContext context) 
    : IHomeRepository
{
    private readonly IscasLuneContext _context = context;

    public async Task<HomeViewModel?> GetHomeAsync()
    {
        var vendasNoMes = await GetQuantidadeVendasNoMesAsync();
        var produtosMaisVendidos = await GetProdutosMaisVendidosAsync();

        return new HomeViewModel()
        {
            QuantidadeVendasNoMes = vendasNoMes,
            ProdutosMaisVendidosViewModel = produtosMaisVendidos
        };
    }

    public async Task<IList<HomeProdutosMaisVendidosViewModel>> GetProdutosMaisVendidosAsync()
    {
        var produtosMaisVendidos = await _context
            .ProdutosMaisVendidos
            .AsNoTracking()
            .OrderByDescending(x => x.QuantidadeProdutos)
                .ThenByDescending(x => x.QuantidadePedidos)
            .Select(x =>
                new ProdutosMaisVendidos(
                    x.Id,
                    x.DataCriacao,
                    x.DataAtualizacao,
                    x.Numero,
                    x.QuantidadeProdutos / x.QuantidadePedidos,
                    x.QuantidadePedidos,
                    x.ProdutoId)
            )
            .ToListAsync();

        produtosMaisVendidos =
        [
            .. produtosMaisVendidos
                        .OrderByDescending(x => x.QuantidadeProdutos)
                            .ThenByDescending(x => x.QuantidadePedidos)
                        .Take(3)
,
        ];

        var produtosIds = produtosMaisVendidos.Select(x => x.ProdutoId).ToList();

        return await _context
            .Produtos
            .AsNoTracking()
            .Where(x => produtosIds.Contains(x.Id))
            .Select(x =>
                new HomeProdutosMaisVendidosViewModel()
                {
                    Id = x.Id,
                    Descricao = x.Descricao,
                    Foto = Encoding.UTF8.GetString(x.Foto),
                    Numero = x.Numero,
                    Referencia = x.Referencia ?? ""
                })
            .ToListAsync();
    }

    public async Task<int> GetQuantidadeVendasNoMesAsync()
    {
        return await _context
            .Pedidos
            .AsNoTracking()
            .Where(x => x.DataCriacao.Month == DateTime.Now.Month)
            .CountAsync();
    }
}
