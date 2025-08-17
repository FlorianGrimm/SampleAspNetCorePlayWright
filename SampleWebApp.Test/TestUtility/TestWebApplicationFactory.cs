namespace SampleWebApp.Test.TestUtility;

public class TestWebApplicationFactory
    : WebApplicationFactory<Program>, TUnit.Core.Interfaces.IAsyncInitializer {
    public TestWebApplicationFactory() {
        this.UseKestrel();
    }

    public TestAuthenticationUsers GetTestAuthenticationUser() => this.Services.GetRequiredService<TestAuthenticationUsers>();

    public async Task InitializeAsync() {
        ProgramTest.Enable();
        this.StartServer();

        await Task.Delay(10).ConfigureAwait(false);
        await this.TestPing();
    }

    public Flurl.Url GetBaseAddress() {
        return new Flurl.Url(this.ClientOptions.BaseAddress);
    }

    public async Task TestPing() {
        using var client = this.CreateClient();
        using var request = new HttpRequestMessage() {
            Method = HttpMethod.Get,
            RequestUri = new Uri("/test/ping", UriKind.Relative)
        };
        using var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        var stringContent = await response.Content.ReadAsStringAsync();

        await Assert.That(stringContent).IsEqualTo("/test/pong");
    }

    public async Task TestSignIn(string authenticationValue) {
        using var client = this.CreateClient();
        client.DefaultRequestHeaders.Authorization
            = new System.Net.Http.Headers.AuthenticationHeaderValue(
                scheme: "TestScheme",
                parameter: authenticationValue
                );

        using var request = new HttpRequestMessage() {
            Method = HttpMethod.Get,
            RequestUri = new Uri("/test/sign-in"),

        };
        using var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();
        var stringContent = await response.Content.ReadAsStringAsync();

        await Assert.That(stringContent).IsEqualTo("/test/pong");
    }
}
