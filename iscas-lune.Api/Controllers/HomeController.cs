using iscas_lune.Api.Application.Interfaces;
using iscas_lune_api.Attributes;
using iscas_lune_api.Discord.Client;
using iscas_lune_api.Exceptions;
using iscaslune.Api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace iscas_lune.Api.Controllers;

[ApiController]
[Route("api/home")]
public class HomeController(IDiscordNotification discordNotification, IHomeService homeService)
    : ControllerBaseIscasLune(discordNotification)
{
    private readonly IHomeService _homeService = homeService;

    [HttpGet("list")]
    [IsFuncionario]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [EnableCors("iscasluneoriginadmin")]
    public async Task<IActionResult> List()
    {
        try
        {
            var home = await _homeService.GetHomeViewModelAsync();
            return Ok(home);
        }
        catch (ExceptionApi ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return await HandleError(ex.Message);
        }
    }
}
