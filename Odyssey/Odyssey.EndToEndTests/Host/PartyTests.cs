using Microsoft.Playwright;
using Microsoft.Playwright.Xunit.v3;

namespace Odyssey.EndToEndTests.Host;

public class PartyTests : PageTest
{
    private const string BaseUrl = "http://localhost:5044";

    [Fact]
    public async Task DisbandPartyChangesJoinCode()
    {
        await Page.GotoAsync($"{BaseUrl}/auth/sign-in");
        await Page.FillAsync("input[name='Username']", "admin");
        await Page.FillAsync("input[name='Password']", "P@ssword1234");
        await Page.ClickAsync("button[type='submit']");
        await Page.WaitForURLAsync($"{BaseUrl}/roles");

        await Page.GotoAsync($"{BaseUrl}/host/party");
        await Expect(Page).ToHaveURLAsync($"{BaseUrl}/host/party");

        var joinCodeLocator = Page.Locator("#host-party-panel .title.uppercase.monospace");
        await Expect(joinCodeLocator).ToBeVisibleAsync();
        var initialJoinCode = await joinCodeLocator.TextContentAsync();

        await Page.ClickAsync("button:has-text('Disband party')");

        await Expect(joinCodeLocator).Not.ToHaveTextAsync(initialJoinCode!);
    }
}
