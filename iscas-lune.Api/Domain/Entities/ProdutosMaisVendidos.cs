using iscaslune.Api.Domain.Entities.Bases;

namespace iscas_lune.Api.Domain.Entities;

public sealed class ProdutosMaisVendidos(
    Guid id,
    DateTime dataCriacao,
    DateTime dataAtualizacao,
    long numero,
    int quantidade,
    Guid produtoId) : BaseEntity(id, dataCriacao, dataAtualizacao, numero)
{
    public int Quantidade { get; private set; } = quantidade;
    public Guid ProdutoId { get; private set; } = produtoId;
}
