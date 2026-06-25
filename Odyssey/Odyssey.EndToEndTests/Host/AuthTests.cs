using Microsoft.Playwright;
using Microsoft.Playwright.Xunit.v3;

namespace Odyssey.EndToEndTests.Host;

public class AuthTests : PageTest
{
    private const string BaseUrl = "http://localhost:5044";

    [Fact]
    public async Task UnauthenticatedAccessToHostRedirectsToSignIn()
    {
        await Page.GotoAsync($"{BaseUrl}/host/");

        await Expect(Page).ToHaveURLAsync($"{BaseUrl}/auth/sign-in?ReturnUrl=%2Fhost%2F");
    }

    [Fact]
    public async Task CanLogInAsHost()
    {
        await Page.GotoAsync($"{BaseUrl}/auth/sign-in");

        await Page.FillAsync("input[name='Username']", "admin");
        await Page.FillAsync("input[name='Password']", "P@ssword1234");
        await Page.ClickAsync("button[type='submit']");

        await Page.WaitForURLAsync($"{BaseUrl}/roles");

        await Page.GotoAsync($"{BaseUrl}/host/");

        await Expect(Page).ToHaveURLAsync($"{BaseUrl}/host/party");
    }
}
