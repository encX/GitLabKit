using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GitLabApiClient;
using GitLabKit.Runner.Core.Exceptions;
using GitLabKit.Runner.Core.Models;
using GitLabKit.Runner.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GitLabKit.Runner.Web.Controllers;

[ApiController]
[Route("runner")]
[Produces("application/json")]
public class RunnerController : ControllerBase
{
    private readonly IGitLabRunnerService _service;

    public RunnerController(IGitLabRunnerService service)
    {
        _service = service;
    }
    
    [HttpGet("group/{groupId:int}")]
    [ProducesResponseType(typeof(IList<Core.Models.Runner>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "GetGroupRunners")]
    public async Task<IActionResult> GetGroupRunners([FromRoute] int groupId)
    {
        var runners = await _service.GetGroupRunners(groupId);
        return Ok(runners.ToList());
    }

    [HttpGet("{runnerId:int}")]
    [ProducesResponseType(typeof(Core.Models.Runner), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "GetSingleRunner")]
    public async Task<IActionResult> GetSingleRunner([FromRoute] int runnerId)
    {
        try
        {
            var runner = await _service.GetSingleRunner(runnerId);
            return Ok(runner);
        }
        catch (GitLabException e)
        {
            if (e.HttpStatusCode == HttpStatusCode.NotFound) return NotFound();
            throw;
        }
    }

    [HttpGet("{runnerId:int}/history")]
    [ProducesResponseType(typeof(IList<Job>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "GetRunnerJobHistory")]
    public async Task<IActionResult> GetRunnerJobHistory([FromRoute] int runnerId)
    {
        try
        {
            var jobs = await _service.GetRunnerJobHistory(runnerId);
            return Ok(jobs.ToList());
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }
    
    [HttpPatch("{runnerId:int}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "SetRunnerActiveStatus")]
    public async Task<IActionResult> SetRunnerActiveStatus([FromRoute] int runnerId, [FromBody] bool isActive)
    {
        try
        {
            var currentStatus = await _service.SetRunnerActiveStatus(runnerId, isActive);
            return Ok(currentStatus);
        }
        catch (GitLabException e)
        {
            if (e.HttpStatusCode == HttpStatusCode.NotFound) return NotFound();
            throw;
        }
    }

    [HttpPatch("bulk")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "BulkSetRunnerActiveStatus")]
    public async Task<IActionResult> BulkSetRunnerActiveStatus([FromBody] BulkRunnerActiveStatus runners)
    {
        await _service.BulkSetRunnerActiveStatus(runners);
        return Ok();
    }
    
    [HttpDelete("{runnerId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "DeleteRunner")]
    public async Task<IActionResult> DeleteRunner([FromRoute] int runnerId)
    {
        try
        {
            await _service.DeleteRunner(runnerId);
            return Ok();
        }
        catch (GitLabException e)
        {
            if (e.HttpStatusCode == HttpStatusCode.NotFound) return NotFound();
            throw;
        }
    }
    
    [HttpPost("bulk-delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "BulkDeleteRunner")]
    public async Task<IActionResult> BulkDeleteRunner([FromBody] BulkRunnerDelete req)
    {
        await _service.BulkDeleteRunner(req.RunnerIds);
        return Ok();
    }
}