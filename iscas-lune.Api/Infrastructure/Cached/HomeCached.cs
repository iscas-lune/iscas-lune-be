using iscas_lune.Api.Infrastructure.Interfaces;
using iscas_lune.Api.Infrastructure.Repositories;
using iscas_lune.Api.Model.Home;
using iscaslune.Api.Application.Interfaces;

namespace iscas_lune.Api.Infrastructure.Cached;

public class HomeCached(ICachedService<HomeViewModel> cachedService, HomeRepository repository)
    : IHomeRepository
{
    private readonly ICachedService<HomeViewModel> _cachedService = cachedService;
    private readonly HomeRepository _repository = repository;
    private const string _key = "home";

    public async Task<int> GetQuantidadeVendasNoMesAsync()
    {
        return await _repository.GetQuantidadeVendasNoMesAsync();
    }

    public async Task<IList<HomeProdutosMaisVendidosViewModel>> GetProdutosMaisVendidosAsync()
    {
        return await _repository.GetProdutosMaisVendidosAsync();
    }

    public async Task<HomeViewModel?> GetHomeAsync()
    {
        var home = await _cachedService.GetItemAsync(_key);

        if (home == null)
        {
            home = new()
            {
                QuantidadeVendasNoMes = await _repository.GetQuantidadeVendasNoMesAsync(),
                ProdutosMaisVendidosViewModel = await _repository.GetProdutosMaisVendidosAsync()
            };

            await _cachedService.SetItemAsync(_key, home);
        }

        return home;
    }
}
