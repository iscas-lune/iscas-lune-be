using iscas_lune.Api.Model.Home;

namespace iscas_lune.Api.Infrastructure.Interfaces;

public interface IHomeRepository
{
    Task<HomeViewModel?> GetQuantidadeVendasNoMesAsync();
}
