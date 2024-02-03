using iscas_lune.Api.Application.Interfaces;
using iscas_lune_api.Discord.Client;
using iscas_lune_api.Exceptions;
using iscaslune.Api.Controllers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace iscas_lune.Api.Controllers;

[ApiController]
[Route("api/home/ecommerce")]
public class HomeEcommerceController(
    IDiscordNotification discordNotification, 
    IHomeEcommerceService homeEcommerceService) 
        : ControllerBaseIscasLune(discordNotification)
{
    private readonly IHomeEcommerceService _homeEcommerceService = homeEcommerceService;

    [EnableCors("iscasluneorigin")]
    [HttpGet("list")]
    public async Task<IActionResult> ListHomeEcommerce()
    {
        try
        {
            var home = await _homeEcommerceService.GetHomeEcommerceAsync();
            return Ok(home);
        }
        catch(ExceptionApi ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return await HandleError(ex.Message);
        }
    }
}
