using iscas_lune_api.Discord.Client;
using iscaslune.Api.Application.Interfaces;
using iscaslune.Api.Dtos.Banners;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace iscaslune.Api.Controllers;

[ApiController]
[Route("api/banner")]

public class BannerController(IBannerService bannerService, IDiscordNotification discordNotification)
        : ControllerBaseIscasLune(discordNotification)
{
    private readonly IBannerService _bannerService = bannerService;

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
}
