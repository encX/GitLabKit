using GitLabApiClient.Models.Runners.Requests;
using GitLabKit.Runner.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace GitLabKit.Runner.Mock.Controllers;

[ApiController]
[Route("/runners/{runnerId:int}")]
public class RunnerController : ControllerBase
{
    [HttpGet]
    public ActionResult<Core.Models.Runner> GetSingleRunner([FromRoute] int runnerId) =>
        Generator.GetMockRunner(runnerId);

    [HttpGet("jobs")]
    public ActionResult<IEnumerable<Job>> GetRunnerJobs([FromRoute] int runnerId, [FromQuery] string status)
    {
        if (status != "running")
        {
            return Enumerable.Range(201, 15)
                .Select(id => Generator.GetMockJob(id, id % 7 == 0 ? "cancelled" : id % 5 == 0 ? "failed" : "success"))
                .ToList();
        }

        if (runnerId % 5 == 0)
        {
            return new List<Job>
            {
                Generator.GetMockJob(123, "running")
            };
        }

        return new List<Job>();
    }

    [HttpPut]
    public ActionResult<Core.Models.Runner> UpdateRunner(
        [FromRoute] int runnerId,
        [FromBody] UpdateRunnerRequest request)
    {
        var runner = Generator.GetMockRunner(runnerId);

        runner.Active = request.Active ?? runner.Active;

        return runner;
    }

    [HttpDelete]
    public ActionResult DeleteRunner([FromRoute] int runnerId)
    {
        return Ok();
    }
}