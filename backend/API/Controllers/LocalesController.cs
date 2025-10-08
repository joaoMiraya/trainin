using API.Application.Interfaces;
using API.Application.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
public class LocalesController : ControllerBase
{
    private readonly ILocalesService _localesService;

    public LocalesController(ILocalesService localesService)
    {
        _localesService = localesService;
    }

    [HttpGet("{*fileName}")]
    public IActionResult GetLocaleFile(string fileName)
    {
        var result = _localesService.GetLocales(fileName);

        if (result == null)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("language/{languageCode}")]
    public IActionResult GetLocalesByLanguage(string languageCode)
    {
        var result = _localesService.GetLocalesByLanguage(languageCode);

        if (result == null)
            return NotFound(result);

        return Ok(result);
    }
}