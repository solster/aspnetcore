using System.Threading.Channels;
using AwesomeAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace Solster.AspNetCore.ActivityLog.IntegrationTests.Steps;

[Binding]
public sealed class ActivityChannelProcessorSteps(ScenarioContext ctx)
{
    private const short SubmissionCreatedAction = 1;
    private const short EventCategory = 1;

    private readonly List<ActivityLogEntry> _entries = [];

    [Given(@"an ActivityLogEntry for resource ""(.*)""")]
    public void GivenEntry(string resourceId) =>
        _entries.Add(new ActivityLogEntry
        {
            Id = Guid.NewGuid(),
            Actor = "system",
            ResourceType = "Submission",
            ResourceId = resourceId,
            Action = SubmissionCreatedAction,
            Category = EventCategory,
            Timestamp = DateTime.UtcNow
        });

    [Given(@"(\d+) ActivityLogEntries for resource ""(.*)""")]
    public void GivenMultipleEntries(int count, string resourceId)
    {
        for (var i = 0; i < count; i++)
        {
            GivenEntry(resourceId);
        }
    }

    [When(@"the entry is written to the channel and the processor runs")]
    [When(@"all entries are written to the channel and the processor runs")]
    public async Task WhenEntriesProcessed()
    {
        var provider = ctx.Get<ServiceProvider>();
        var channel = provider.GetRequiredService<Channel<ActivityLogEntry>>();

        foreach (var entry in _entries)
        {
            await channel.Writer.WriteAsync(entry);
        }

        channel.Writer.Complete();

        var processor = new ActivityChannelProcessor(
            channel.Reader,
            provider.GetRequiredService<IServiceScopeFactory>(),
            NullLogger<ActivityChannelProcessor>.Instance);

        await processor.StartAsync(CancellationToken.None);
        await processor.ExecuteTask!;
    }

    [Then(@"(\d+) activity log entry exists for resource ""(.*)""")]
    [Then(@"(\d+) activity log entries exist for resource ""(.*)""")]
    public async Task ThenEntriesExist(int count, string resourceId)
    {
        var provider = ctx.Get<ServiceProvider>();
        var store = provider.GetRequiredService<IActivityStore>();

        var entries = await store.GetActivityForResourceAsync("Submission", resourceId, take: 1000);

        entries.Should().HaveCount(count);
    }
}
