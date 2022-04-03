using System.Net;
using System.Threading.Tasks;
using GitLabApiClient;
using GitLabKit.Runner.Core.Models;
using GitLabKit.Runner.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GitLabKit.Runner.Web.Controllers;

[ApiController]
[Route("group")]
[Produces("application/json")]
public class GroupController : ControllerBase
{
    private readonly IGitLabGroupService _groupService;

    public GroupController(IGitLabGroupService groupService)
    {
        _groupService = groupService;
    }
    
    [HttpGet("{groupId:int}")]
    [ProducesResponseType(typeof(Group), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(OperationId = "GetGroup")]
    public async Task<IActionResult> GetGroup([FromRoute] int groupId)
    {
        try
        {
            var group = await _groupService.GetGroup(groupId);
            return Ok(group);
        }
        catch (GitLabException e)
        {
            if (e.HttpStatusCode == HttpStatusCode.NotFound) return NotFound();
            throw;
        }
    }
}