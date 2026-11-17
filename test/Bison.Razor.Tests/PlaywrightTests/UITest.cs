using Microsoft.Playwright.Xunit;
using Xunit;

namespace PlaywrightTests;

public class Tests : PageTest
{
    [Fact]
    public async Task Obs_is_hidden_when_logged_out()
    {
        await Page.GotoAsync("http://localhost:5273");

        await Expect(Page.Locator(".observationbox")).ToHaveCountAsync(0);
    }
}