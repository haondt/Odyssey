using Microsoft.Playwright;
using Microsoft.Playwright.Xunit.v3;

namespace Odyssey.EndToEndTests.Host;

public class AuthRedirectTests : PageTest
{
    private const string BaseUrl = "http://localhost:5044";

    [Fact]
    public async Task UnauthenticatedAccessToHostRedirectsToSignIn()
    {
        await Page.GotoAsync($"{BaseUrl}/host/");

        await Expect(Page).ToHaveURLAsync($"{BaseUrl}/auth/sign-in?ReturnUrl=%2Fhost%2F");
    }
}
