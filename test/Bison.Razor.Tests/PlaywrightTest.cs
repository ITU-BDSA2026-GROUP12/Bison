using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

namespace PlaywrightTests;

public class UnitTest1 : PageTest {
    [Fact]
    public async Task HasTitle() {
        //go to our website
        await Page.GotoAsync("https://bdsagroup12bison-ade4hjeqa2f8agam.denmarkeast-01.azurewebsites.net/");

        //Title should include Bison
        await Expect(Page).ToHaveTitleAsync(new Regex("Bison"));
    }

    [Fact]
    public async Task GetStartedLink() {
        await Page.GotoAsync("https://bdsagroup12bison-ade4hjeqa2f8agam.denmarkeast-01.azurewebsites.net/");

        //click the get Login link.
        await Page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();

        // Expect page to have a heading with the name of Log in.
        await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "Log in", Level = 1 })).ToBeVisibleAsync();
    }
}