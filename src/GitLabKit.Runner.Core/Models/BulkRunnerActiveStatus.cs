using System.Collections.Generic;

namespace GitLabKit.Runner.Core.Models;

public class BulkRunnerActiveStatus
{
    public IDictionary<int, bool> Runners { get; set; }
}