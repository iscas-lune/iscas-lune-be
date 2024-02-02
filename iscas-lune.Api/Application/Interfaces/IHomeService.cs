using iscas_lune.Api.Model.Home;

namespace iscas_lune.Api.Application.Interfaces;

public interface IHomeService
{
    Task<HomeViewModel> GetHomeViewModelAsync();
}
