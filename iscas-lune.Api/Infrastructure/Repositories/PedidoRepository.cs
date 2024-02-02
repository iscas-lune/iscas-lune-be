using iscas_lune_api.Domain.Entities;
using iscas_lune_api.Dtos.Pedidos;
using iscas_lune_api.Infrastructure.Interfaces;
using iscas_lune_api.Model.Paginacao;
using iscas_lune_api.Model.Pedidos;
using iscaslune.Api.Domain.Context;
using iscaslune.Api.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using iscas_lune.Api.Infrastructure.Filtros.IQuerable;

namespace iscas_lune_api.Infrastructure.Repositories;

public class PedidoRepository : GenericRepository<Pedido>, IPedidoRepository
{
    private readonly IscasLuneContext _context;
    public PedidoRepository(IscasLuneContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PaginacaoViewModel<Pedido>> GetPaginacaoPedidoAsync(PaginacaoPedidoDto paginacaoPedidoDto)
    {

        var (total, values) = await _context
                .Pedidos
                .AsNoTracking()
                .AsQueryable()
                .OrderByDescending(x => EF.Property<Pedido>(x, paginacaoPedidoDto.OrderBy))
                .Include(x => x.Usuario)
                .WhereIsNotNull(paginacaoPedidoDto.GetWhereBySearch())
                .WhereIsNotNull(paginacaoPedidoDto.GetWhereByStatusPedido())
                .CustomFilterAsync(paginacaoPedidoDto);

        return new()
        {
            TotalPage = total,
            Values = values
        };
    }

    public async Task<Pedido?> GetPedidoByIdAsync(Guid id)
    {
        return await _context.Pedidos
            .AsNoTracking()
            .Include(x => x.ItensPedido)
                .ThenInclude(x => x.Produto)
            .Include(x => x.Usuario)
            .Include(x => x.ItensPedido)
                .ThenInclude(x => x.Tamanho)
            .Include(x => x.ItensPedido)
                .ThenInclude(x => x.Peso)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Pedido?> GetPedidoByUpdateStatusAsync(Guid id)
    {
        return await _context.Pedidos.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<PedidoViewModelSemItens>> GetPedidosByUsuarioIdAsync(Guid usuarioId, int statusPedido)
    {
        return await _context.Pedidos
            .AsNoTracking()
            .OrderByDescending(x => x.Numero)
            .AsQueryable()
            .Where(x => x.UsuarioId == usuarioId && x.StatusPedido == (StatusPedido)statusPedido)
            .Select(pedido => new PedidoViewModelSemItens()
            {
                Numero = pedido.Numero,
                DataCriacao = pedido.DataCriacao,
                Id = pedido.Id,
                StatusPedido = pedido.StatusPedido
            })
            .ToListAsync();
    }
}
