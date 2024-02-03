using iscas_lune.Api.Application.Interfaces;
using iscas_lune.Api.Infrastructure.Interfaces;
using iscas_lune.Api.Model.Home;
using iscas_lune_api.Exceptions;

namespace iscas_lune.Api.Application.Services;

public class HomeService : IHomeService
{
    private readonly IHomeRepository _homeRepository;

    public HomeService(IHomeRepository homeRepository)
    {
        _homeRepository = homeRepository;
    }

    public async Task<HomeViewModel> GetHomeViewModelAsync()
    {
        return await _homeRepository.GetHomeAsync()
            ?? throw new ExceptionApi();
    }
}
