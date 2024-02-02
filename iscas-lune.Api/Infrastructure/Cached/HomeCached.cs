using iscas_lune.Api.Infrastructure.Interfaces;
using iscas_lune.Api.Infrastructure.Repositories;
using iscas_lune.Api.Model.Home;
using iscaslune.Api.Application.Interfaces;

namespace iscas_lune.Api.Infrastructure.Cached;

public class HomeCached : IHomeRepository
{
    private readonly ICachedService<HomeViewModel> _cachedService;
    private readonly HomeRepository _repository;
    private const string _key = "home";

    public HomeCached(ICachedService<HomeViewModel> cachedService, HomeRepository repository)
    {
        _cachedService = cachedService;
        _repository = repository;
    }

    public async Task<HomeViewModel?> GetQuantidadeVendasNoMesAsync()
    {
        var home = await _cachedService.GetItemAsync(_key);

        if(home == null)
        {
            home = await _repository.GetQuantidadeVendasNoMesAsync();
            if(home != null) 
                await _cachedService.SetItemAsync(_key, home);
        }

        return home;
    }
}
