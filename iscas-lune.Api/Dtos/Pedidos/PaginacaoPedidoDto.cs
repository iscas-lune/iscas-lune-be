using iscas_lune_api.Domain.Entities;
using iscaslune.Api.Infrastructure.Filters.Model;
using System.Linq.Expressions;

namespace iscas_lune_api.Dtos.Pedidos;
 
public class PaginacaoPedidoDto : FilterModel
{
    public int? StatusPedido { get; set; }

    public Expression<Func<Pedido, bool>>? GetWhereBySearch()
    {
        if (string.IsNullOrWhiteSpace(Search))
            return null;

        return x => x.Usuario.Nome.ToLower().Contains(Search.ToLower());
    }

    public Expression<Func<Pedido, bool>>? GetWhereByStatusPedido()
    {
        if (StatusPedido == null)
            return null;

        return x => x.StatusPedido == (StatusPedido)StatusPedido;
    }
}
