using iscas_lune.Api.Dtos.Banners;
using iscas_lune_api.Exceptions;
using iscas_lune_api.Model.Paginacao;
using iscaslune.Api.Application.Interfaces;
using iscaslune.Api.Dtos.Banners;
using iscaslune.Api.Infrastructure.Interfaces;
using iscaslune.Api.Model.Banners;

namespace iscaslune.Api.Application.Services;

public class BannerService(IBannerRepository bannerRepository)
        : IBannerService
{
    private readonly IBannerRepository _bannerRepository = bannerRepository;

    public async Task<BannerViewModel?> CreateBannerAsync(CreateBannerDto createBannerDto)
    {
        var banner = createBannerDto.ForEntity();
        var result = await _bannerRepository.AddAsync(banner);
        if (!result) return null;
        return new BannerViewModel().ForModel(banner);
    }

    public async Task<bool> DeleteBannerAsync(Guid id)
    {
        var banner = await _bannerRepository.GetBannerByIdAsync(id)
            ?? throw new ExceptionApi("Banner não foi localizado!");

        return await _bannerRepository.DeleteAsync(banner);
    }

    public async Task<List<BannerViewModel>> GetBannersAsync()
    {
        var banners = await _bannerRepository.GetBannerAsync();
        return banners.Select(x => new BannerViewModel().ForModel(x) ?? new()).ToList();
    }

    public async Task<PaginacaoViewModel<BannerViewModel>> GetPaginacaoAsync(PaginacaoBannerDto paginacaoBannerDto)
    {
        var paginacao = await _bannerRepository.GetPaginacaoBannerAsync(paginacaoBannerDto);

        return new()
        {
            TotalPage = paginacao.TotalPage,
            Values = paginacao.Values.Select(x => new BannerViewModel().ForModel(x) ?? new()).ToList()
        };
    }
}
