using iscas_lune.Api.Model.Home;
using System.Threading.Tasks;

namespace iscas_lune.Api.Infrastructure.Interfaces;

public interface IHomeRepository
{
    Task<int> GetQuantidadeVendasNoMesAsync();
    Task<IList<HomeProdutosMaisVendidosViewModel>> GetProdutosMaisVendidosAsync();
    Task<HomeViewModel?> GetHomeAsync();
}
