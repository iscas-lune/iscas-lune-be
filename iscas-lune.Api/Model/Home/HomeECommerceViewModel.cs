using iscaslune.Api.Model.Banners;
using iscaslune.Api.Model.Categorias;

namespace iscas_lune.Api.Model.Home;

public class HomeECommerceViewModel
{
    public IList<BannerViewModel> Banners { get; set; } = new List<BannerViewModel>();
    public IList<CategoriaViewModel> Categorias { get; set; } = new List<CategoriaViewModel>();
    public IList<HomeProdutosMaisVendidosViewModel> ProdutosMaisVendidos { get; set; } = new List<HomeProdutosMaisVendidosViewModel>();
}
