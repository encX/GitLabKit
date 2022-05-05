using GitLabApiClient.Models.Runners.Requests;
using GitLabKit.Runner.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace GitLabKit.Runner.Mock.Controllers;

[ApiController]
[Route("/api/v4/runners/{runnerId:int}")]
public class RunnerController : ControllerBase
{
    [HttpGet]
    public ActionResult<Core.Models.Runner> GetSingleRunner([FromRoute] int runnerId) =>
        Generator.GetMockRunner(runnerId);

    [HttpGet("jobs")]
    public ActionResult<IEnumerable<Job>> GetRunnerJobs(
        [FromRoute] int runnerId,
        [FromQuery(Name = "status")] string? queryStatus,
        [FromQuery] int? page)
    {
        if (page > 1) return new List<Job>();

        if (queryStatus != "running")
        {
            var jobs = Enumerable.Range(201, 15)
                .Select(id => Generator.GetMockJob(id, id % 7 == 0 ? "cancelled" : id % 5 == 0 ? "failed" : "success"))
                .ToList();

            if (IsARunningRunner(runnerId)) jobs.Add(Generator.GetMockJob(112, "running"));

            return jobs;
        }

        if (queryStatus == "running" && IsARunningRunner(runnerId))
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

    private static bool IsARunningRunner(int runnerId) => runnerId % 5 == 0;
}