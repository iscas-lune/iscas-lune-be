using iscas_lune.Api.Dtos.Banners;
using iscas_lune_api.Attributes;
using iscas_lune_api.Discord.Client;
using iscas_lune_api.Dtos.Pedidos;
using iscas_lune_api.Exceptions;
using iscaslune.Api.Application.Interfaces;
using iscaslune.Api.Dtos.Banners;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace iscaslune.Api.Controllers;

[ApiController]
[Route("api/banner")]

public class BannerController(IBannerService bannerService, IDiscordNotification discordNotification)
        : ControllerBaseIscasLune(discordNotification)
{
    private readonly IBannerService _bannerService = bannerService;

    [Authorize(AuthenticationSchemes = "Bearer")]
    [IsFuncionario]
    [EnableCors("iscasluneoriginadmin")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateBanner(CreateBannerDto createBannerDto)
    {
		try
		{
            var result = await _bannerService.CreateBannerAsync(createBannerDto);
            return HandleCreated(result, $"api/banner/get?id={result?.Id}");
		}
		catch (Exception ex)
		{
            return await HandleError(ex.Message);
		}
    }

    [EnableCors("iscasluneorigin")]
    [HttpGet("list")]
    public async Task<IActionResult> ListBanner()
    {
        try
        {
            var result = await _bannerService.GetBannersAsync();
            return HandleGet(result);
        }
        catch (Exception ex)
        {
            return await HandleError(ex.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [IsFuncionario]
    [EnableCors("iscasluneoriginadmin")]
    [HttpGet("paginacao")]
    public async Task<IActionResult> Paginacao([FromQuery] PaginacaoBannerDto paginacaoBannerDto)
    {
        try
        {
            var paginacaoViewModel = await _bannerService.GetPaginacaoAsync(paginacaoBannerDto);
            return Ok(paginacaoViewModel);
        }
        catch (ExceptionApi ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return await HandleError(ex.Message);
        }
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [IsFuncionario]
    [EnableCors("iscasluneoriginadmin")]
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromQuery] Guid id)
    {
        try
        {
            var result = await _bannerService.DeleteBannerAsync(id);

            if (!result) return BadRequest(new { message = "Ocorreu um erro interno, tente novamente mais tarde!" });

            return Ok();
        }
        catch (ExceptionApi ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return await HandleError(ex.Message);
        }
    }
}
