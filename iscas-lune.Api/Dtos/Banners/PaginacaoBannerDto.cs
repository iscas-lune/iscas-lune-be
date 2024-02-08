using iscaslune.Api.Domain.Entities;
using iscaslune.Api.Infrastructure.Filters.Model;
using System.Linq.Expressions;

namespace iscas_lune.Api.Dtos.Banners;

public class PaginacaoBannerDto : FilterModel
{
    public Expression<Func<Banner, bool>>? GetWhereBySearch()
    {
        if (string.IsNullOrWhiteSpace(Search))
            return null;

        return x => x.Numero.ToString().ToLower().Contains(Search.ToLower());
    }
}
