using iscas_lune.Api.Dtos.Banners;
using iscas_lune_api.Model.Paginacao;
using iscaslune.Api.Application.Interfaces;
using iscaslune.Api.Domain.Context;
using iscaslune.Api.Domain.Entities;
using iscaslune.Api.Infrastructure.Interfaces;
using iscaslune.Api.Infrastructure.Repositories;

namespace iscaslune.Api.Infrastructure.Cached;

public class BannerCached
    : GenericRepository<Banner>, IBannerRepository
{
    private readonly BannerRepository _bannerRepository;
    private readonly ICachedService<Banner> _cachedService;
    private const string _key = "banners";

    public BannerCached(IscasLuneContext context, BannerRepository bannerRepository, ICachedService<Banner> cachedService) : base(context)
    {
        _bannerRepository = bannerRepository;
        _cachedService = cachedService;
    }

    public async Task<List<Banner>> GetBannerAsync()
    {
        var banners = await _cachedService.GetListItemAsync(_key);

        if (banners == null)
        {
            banners = await _bannerRepository.GetBannerAsync();
            if (banners.Count > 0)
                await _cachedService.SetListItemAsync(_key, banners);
        }

        return banners;
    }

    public async Task<PaginacaoViewModel<Banner>> GetPaginacaoBannerAsync(PaginacaoBannerDto paginacaoBannerDto)
    {
        return await _bannerRepository.GetPaginacaoBannerAsync(paginacaoBannerDto);
    }

    public async Task<Banner?> GetBannerByIdAsync(Guid id)
    {
        var key = $"{id}";
        var banner = await _cachedService.GetItemAsync(key);

        if(banner == null)
        {
            banner = await _bannerRepository.GetBannerByIdAsync(id);
            if(banner != null)
                await _cachedService.SetItemAsync(key, banner);
        }

        return banner;
    }
}
