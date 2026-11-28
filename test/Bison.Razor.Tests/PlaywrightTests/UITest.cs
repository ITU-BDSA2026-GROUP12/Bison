using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
using Xunit;
namespace PlaywrightTests;

public class Tests : PageTest
{
        // this is the url i just dont want to type is all test long. It should be local on when testing local but out azure domain when we push it.
        string url = "http://localhost:5273";

    public async Task Register(string name, string email, string password)
    {
        await Page.GotoAsync($"{url}/Identity/Account/Register");

        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Name" }).FillAsync(name);
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync(email);
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Password", Exact = true }).FillAsync(password);
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Confirm Password" }).FillAsync(password);
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your" }).ClickAsync();
        await Expect(Page.GetByText("Thank you for confirming")).ToBeVisibleAsync();
    }

    private async Task Login(string email, string password)
    {
        await Page.GotoAsync($"{url}/Identity/Account/Login");
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync(email);
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync(password);
        await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();

        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Logout" })).ToBeVisibleAsync();
    }

    [Fact]
    public async Task Obs_is_hidden_when_logged_out()
    {
        await Page.GotoAsync(url);

        await Expect(Page.Locator(".observationbox")).ToHaveCountAsync(0);
    }

    [Fact]
    public async Task Obs_is_visible_when_logged_in()

    {
        // this is a smart one so we generate a new name every time we run a test. Then we use the name paramter inside email so in one go we basically get a new email and a new name
        string name = $"user{Guid.NewGuid():N}"[..12];
        string email = name +"@itu.dk";
        string password = "ronaldodagoat";


        await Register(name, email, password);
        await Login(email, password);
        await Page.GotoAsync(url);
        

        await Expect(Page.Locator(".observationbox")).ToBeVisibleAsync();
    }
}