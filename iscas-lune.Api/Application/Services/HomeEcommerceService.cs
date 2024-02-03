using iscas_lune.Api.Application.Interfaces;
using iscas_lune.Api.Infrastructure.Interfaces;
using iscas_lune.Api.Model.Home;
using iscaslune.Api.Dtos.Categorias;
using iscaslune.Api.Infrastructure.Interfaces;
using iscaslune.Api.Model.Banners;
using iscaslune.Api.Model.Categorias;

namespace iscas_lune.Api.Application.Services;

public class HomeEcommerceService(
    IHomeRepository homeRepository, 
    ICategoriaRepository categoryRepository, 
    IBannerRepository bannerRepository) 
    : IHomeEcommerceService
{
    private readonly IHomeRepository _homeRepository = homeRepository;
    private readonly ICategoriaRepository _categoryRepository = categoryRepository;
    private readonly IBannerRepository _bannerRepository = bannerRepository;

    public async Task<HomeECommerceViewModel> GetHomeEcommerceAsync()
    {
        var categorias = await _categoryRepository.GetCategoriasAsync(new PaginacaoCategoriaDto());
        var banners = await _bannerRepository.GetBannerAsync();
        var home = await _homeRepository.GetHomeAsync();

        var categoriasView = categorias?
            .Select(x => 
                new CategoriaViewModel().ForModel(x) ?? new())
            .ToList();

        var bannersViewModel = banners?
            .Select(x => 
                new BannerViewModel().ForModel(x) ?? new())
            .ToList();

        return new()
        {
            Banners = bannersViewModel ?? [],
            Categorias = categoriasView ?? [],
            ProdutosMaisVendidos = home?.ProdutosMaisVendidosViewModel 
                ?? new List<HomeProdutosMaisVendidosViewModel>()
        };
    }
}
