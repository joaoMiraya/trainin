using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly ILogger<HealthController> _logger;

    public HealthController(ILogger<HealthController> logger)
    {
        _logger = logger;
    }

    [HttpGet("error")]
    public IActionResult GetError()
    {
        try
        {
            throw new InvalidOperationException("Erro de teste");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar requisição");
            return StatusCode(500, "Erro interno do servidor");
        }
    }
}
