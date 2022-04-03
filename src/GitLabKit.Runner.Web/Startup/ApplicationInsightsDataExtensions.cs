using System.Collections.Generic;
using Microsoft.ApplicationInsights.Extensibility.EventCounterCollector;
using Microsoft.Extensions.DependencyInjection;

namespace GitLabKit.Runner.Web.Startup;

public static class ApplicationInsightsDataExtensions
{
    public static void EnrichAppInsightsData(this IServiceCollection service)
    {
        service.ConfigureTelemetryModule<EventCounterCollectionModule>((module, options) =>
        {
            foreach (var (_, counter) in SystemRuntimeCounters.Counters)
            {
                module.Counters.Add(new EventCounterCollectionRequest(SystemRuntimeCounters.Name, counter));
            }
        });
    }

    private static readonly EventSource SystemRuntimeCounters = new(
        "System.Runtime",
        new Dictionary<string, string>
        {
            {"CpuTimeCounter", "cpu-usage"},
            {"WorkingSetCounter", "working-set"},
            {"GcHeapSizeCounter", "gc-heap-size"},
            {"Gen0GCCounter", "gen-0-gc-count"},
            {"Gen1GCCounter", "gen-1-gc-count"},
            {"Gen2GCCounter", "gen-2-gc-count"},
            {"ThreadPoolThreadCounter", "threadpool-thread-count"},
            {"MonitorContentionCounter", "monitor-lock-contention-count"},
            {"ThreadPoolQueueCounter", "threadpool-queue-length"},
            {"CompletedItemsCounter", "threadpool-completed-items-count"},
            {"AllocRateCounter", "alloc-rate"},
            {"TimerCounter", "active-timer-count"},
            {"FragmentationCounter", "gc-fragmentation"},
            {"CommittedCounter", "gc-committed"},
            {"ExceptionCounter", "exception-count"},
            {"GcTimeCounter", "time-in-gc"},
            {"Gen0SizeCounter", "gen-0-size"},
            {"Gen1SizeCounter", "gen-1-size"},
            {"Gen2SizeCounter", "gen-2-size"},
            {"LohSizeCounter", "loh-size"},
            {"PohSizeCounter", "poh-size"},
            {"AssemblyCounter", "assembly-count"},
            {"IlBytesJittedCounter", "il-bytes-jitted"},
            {"MethodsJittedCounter", "methods-jitted-count"},
            {"JitTimeCounter", "time-in-jit"}
        });

    private record EventSource(string Name, Dictionary<string, string> Counters)
    {
        public Dictionary<string, string> Counters { get; } = Counters;
        public string Name { get; } = Name;
    }
}