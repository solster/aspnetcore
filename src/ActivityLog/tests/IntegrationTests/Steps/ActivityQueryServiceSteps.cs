using AwesomeAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Solster.AspNetCore.ActivityLog.IntegrationTests.Steps;

[Binding]
public sealed class ActivityQueryServiceSteps(ScenarioContext ctx)
{
    private const short EventCategory = 1;

    private IReadOnlyList<ActivityLogEntry>? _result;

    [Given(@"the activity database is empty")]
    public void GivenDatabaseIsEmpty()
    {
        // handled by fresh SQLite db per scenario in TestHooks
    }

    [Given(@"the following activity entries exist:")]
    public async Task GivenActivityEntriesExist(DataTable table)
    {
        var provider = ctx.Get<ServiceProvider>();
        var store = provider.GetRequiredService<IActivityStore>();

        foreach (var row in table.Rows)
        {
            await store.AppendAsync(new ActivityLogEntry
            {
                Id = Guid.NewGuid(),
                Actor = row["Actor"],
                ResourceType = row["ResourceType"],
                ResourceId = row["ResourceId"],
                Action = short.Parse(row["Action"]),
                Category = EventCategory,
                Timestamp = DateTime.Parse(row["Timestamp"]).ToUniversalTime()
            });
        }
    }

    [When(@"GetGlobalActivityAsync is called with take (\d+)")]
    public async Task WhenGetGlobalActivityIsCalled(int take)
    {
        var provider = ctx.Get<ServiceProvider>();
        var svc = provider.GetRequiredService<IActivityQueryService>();
        _result = await svc.GetGlobalActivityAsync(take);
    }

    [When(@"GetActivityForResourceAsync is called for resource type ""(.*)"" and id ""(.*)"" with take (\d+)")]
    public async Task WhenGetActivityForResourceIsCalled(string resourceType, string resourceId, int take)
    {
        var provider = ctx.Get<ServiceProvider>();
        var svc = provider.GetRequiredService<IActivityQueryService>();
        _result = await svc.GetActivityForResourceAsync(resourceType, resourceId, take);
    }

    [Then(@"(\d+) entries are returned")]
    public void ThenEntriesAreReturned(int count) =>
        _result.Should().HaveCount(count);

    [Then(@"the first entry has resource id ""(.*)""")]
    public void ThenFirstEntryHasResourceId(string expected) =>
        _result![0].ResourceId.Should().Be(expected);

    [Then(@"the last entry has resource id ""(.*)""")]
    public void ThenLastEntryHasResourceId(string expected) =>
        _result![^1].ResourceId.Should().Be(expected);

    [Then(@"all entries have resource id ""(.*)""")]
    public void ThenAllEntriesHaveResourceId(string expected) =>
        _result.Should().AllSatisfy(e => e.ResourceId.Should().Be(expected));
}
