using iscas_lune.Api.Model.Home;

namespace iscas_lune.Api.Application.Interfaces;

public interface IHomeEcommerceService
{
    Task<HomeECommerceViewModel> GetHomeEcommerceAsync();
}
