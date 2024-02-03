namespace iscas_lune.Api.Model.Home;

public class HomeViewModel
{
    public int QuantidadeVendasNoMes { get; set; }
    public IList<HomeProdutosMaisVendidosViewModel> ProdutosMaisVendidosViewModel { get; set; } = 
        new List<HomeProdutosMaisVendidosViewModel>();
}
