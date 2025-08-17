namespace SampleWebApp.Test;
public class Tests : SamplePageTest {

    [Test, TUnit.Core.Explicit]
    public async Task Test1() {
        await Page.GotoAsync("https://playwright.dev");


        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));

        // create a locator
        var getStarted = Page.Locator("text=Get Started");

        // Expect an attribute "to be strictly equal" to the value.
        await Expect(getStarted).ToHaveAttributeAsync("href", "/docs/intro");

        // Click the get started link.
        await getStarted.ClickAsync();

        // Expects the URL to contain intro.
        await Expect(Page).ToHaveURLAsync(new Regex(".*intro"));
    }

    [Test]
    public async Task Test2() {
        using var client = this.Factory.CreateClient();
        Flurl.Url urlRoot = this.Factory.GetBaseAddress();
        var user = this.GetTestAuthenticationUser().UserPlanner1;
        client.DefaultRequestHeaders.Authorization = user.GetAuthenticationHeaderValue();

        Flurl.Url url = urlRoot.Clone().AppendPathSegment("/test/sign-in");
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var stringContent = await response.Content.ReadAsStringAsync();

        await Assert.That(stringContent).Contains(user.Name);
    }

    [Test]
    public async Task Test3() {
        using var client = this.Factory.CreateClient();
        var baseAddress = client.BaseAddress;
        System.Console.Out.WriteLine($"baseAddress: {baseAddress}");

        //var client = WebApplicationFactory.CreateClient();

        using var response = await client.GetAsync("/ping");
        response.EnsureSuccessStatusCode();
        var stringContent = await response.Content.ReadAsStringAsync();

        await Assert.That(stringContent).IsEqualTo("pong");
    }


    [Test]
    public async Task Test4() {
        var user = this.Factory.GetTestAuthenticationUser().UserPlanner1;
        Flurl.Url urlRoot = this.Factory.GetBaseAddress();
        Flurl.Url url = urlRoot.Clone().AppendPathSegment("/test/sign-in")
            .AppendQueryParam("user", user.Name)
            .AppendQueryParam("password", user.Password)
            ;
        await Page.GotoAsync(url);
        await Expect(Page.GetByTestId("home")).ToContainTextAsync(user.Name);
        await Page.GetByTestId("home").ClickAsync();

        Flurl.Url pageUrl = Page.Url;
        Debug.Assert(urlRoot.Equals(pageUrl));
    }

}
