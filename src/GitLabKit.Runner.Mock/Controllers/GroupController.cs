using GitLabKit.Runner.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace GitLabKit.Runner.Mock.Controllers;

[ApiController]
[Route("/api/v4/groups/{groupId:int}")]
public class GroupController : ControllerBase
{
    [HttpGet]
    public ActionResult<Group> GetSingleGroup([FromRoute] int groupId)
    {
        return new Group
        {
            Id = groupId,
            Name = $"My Awesome Group {groupId}",
            WebUrl = "https://gitlab.com"
        };
    }

    [HttpGet("runners")]
    public ActionResult<IEnumerable<Core.Models.Runner>> GetGroupRunners([FromRoute] int groupId, [FromQuery] int? page)
    {
        return page > 1 
            ? new List<Core.Models.Runner>() 
            : Enumerable.Range(2001, 20).Select(Generator.GetMockRunner).ToList();
    }
}