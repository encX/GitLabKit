using GitLabKit.Runner.Core.Repositories;
using NSubstitute;

namespace GitLabKit.Runner.UnitTests;

internal static class Given
{
    internal static IGitLabRepository GitLabRepository => Substitute.For<IGitLabRepository>();
}