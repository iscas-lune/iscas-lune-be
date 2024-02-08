using iscas_lune.Api.Dtos.Banners;
using iscas_lune_api.Model.Paginacao;
using iscaslune.Api.Domain.Entities;

namespace iscaslune.Api.Infrastructure.Interfaces;

public interface IBannerRepository : IGenericRepository<Banner>
{
    Task<List<Banner>> GetBannerAsync();
    Task<Banner?> GetBannerByIdAsync(Guid id);
    Task<PaginacaoViewModel<Banner>> GetPaginacaoBannerAsync(PaginacaoBannerDto paginacaoBannerDto);
}
