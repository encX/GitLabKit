using System.Net;
using GitLabKit.Runner.Core.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GitLabKit.Runner.Web.Controllers;

public class HealthCheckController : ControllerBase
{
    private static readonly AssemblyInfo AssemblyInfo = new AssemblyInfo();
        
    [Route("/api/health")]
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(bool))]
    [SwaggerOperation(OperationId = "index")]
    public IActionResult index()
    {
        return Ok();
    }
        
    [Route("/api/version")]
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(AssemblyInfo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "GetVersion")]
    public IActionResult GetVersion() => Ok(AssemblyInfo);
}