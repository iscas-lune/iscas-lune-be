using iscas_lune_api.Exceptions;
using iscaslune.Api.Domain.Entities.Bases;

namespace iscas_lune.Api.Domain.Entities;

public sealed class ProdutosMaisVendidos(
    Guid id,
    DateTime dataCriacao,
    DateTime dataAtualizacao,
    long numero,
    decimal quantidadeProdutos,
    decimal quantidadePedidos,
    Guid produtoId) : BaseEntity(id, dataCriacao, dataAtualizacao, numero)
{
    public decimal QuantidadeProdutos { get; private set; } = quantidadeProdutos;
    public decimal QuantidadePedidos { get; private set; } = quantidadePedidos;
    public Guid ProdutoId { get; private set; } = produtoId;

    public void UpdateQuantidadeProdutos(decimal quantidadeProdutos)
    {
        if (quantidadeProdutos <= 0)
            throw new ExceptionApi("Quantidade de produtos inválida!");

        QuantidadeProdutos += quantidadeProdutos;
        QuantidadePedidos++;
    }
}
