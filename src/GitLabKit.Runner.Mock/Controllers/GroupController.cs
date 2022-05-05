using GitLabKit.Runner.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace GitLabKit.Runner.Mock.Controllers;

[ApiController]
[Route("/groups/{groupId:int}")]
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
    public ActionResult<IEnumerable<Core.Models.Runner>> GetGroupRunners([FromRoute] int groupId)
    {
        return Enumerable.Range(2001, 20).Select(Generator.GetMockRunner).ToList();
    }
}