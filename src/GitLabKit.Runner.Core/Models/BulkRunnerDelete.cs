using System.Collections.Generic;

namespace GitLabKit.Runner.Core.Models;

public class BulkRunnerDelete
{
    public IEnumerable<int> RunnerIds { get; set; }
}