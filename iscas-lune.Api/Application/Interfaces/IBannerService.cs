using iscas_lune.Api.Dtos.Banners;
using iscas_lune_api.Model.Paginacao;
using iscaslune.Api.Dtos.Banners;
using iscaslune.Api.Model.Banners;

namespace iscaslune.Api.Application.Interfaces;

public interface IBannerService
{
    Task<List<BannerViewModel>> GetBannersAsync();
    Task<BannerViewModel?> CreateBannerAsync(CreateBannerDto createBannerDto);
    Task<PaginacaoViewModel<BannerViewModel>> GetPaginacaoAsync(PaginacaoBannerDto paginacaoBannerDto);
    Task<bool> DeleteBannerAsync(Guid id);
}
