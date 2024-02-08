using iscas_lune.Api.Dtos.Banners;
using iscas_lune.Api.Infrastructure.Filtros.IQuerable;
using iscas_lune_api.Domain.Entities;
using iscas_lune_api.Dtos.Pedidos;
using iscas_lune_api.Model.Paginacao;
using iscaslune.Api.Domain.Context;
using iscaslune.Api.Domain.Entities;
using iscaslune.Api.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iscaslune.Api.Infrastructure.Repositories;

public class BannerRepository(IscasLuneContext context)
        : GenericRepository<Banner>(context), IBannerRepository
{
    private readonly IscasLuneContext _context = context;

    public async Task<List<Banner>> GetBannerAsync()
    {
        return await _context.Banners.AsNoTracking().Where(x => x.Ativo).ToListAsync();
    }

    public async Task<Banner?> GetBannerByIdAsync(Guid id)
    {
        return await _context
            .Banners
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PaginacaoViewModel<Banner>> GetPaginacaoBannerAsync(PaginacaoBannerDto paginacaoBannerDto)
    {
        var (total, values) = await _context
                .Banners
                .AsNoTracking()
                .AsQueryable()
                .OrderByDescending(x => EF.Property<Pedido>(x, paginacaoBannerDto.OrderBy))
                .WhereIsNotNull(paginacaoBannerDto.GetWhereBySearch())
                .CustomFilterAsync(paginacaoBannerDto);

        return new()
        {
            TotalPage = total,
            Values = values
        };
    }
}
