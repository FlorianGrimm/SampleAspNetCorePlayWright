namespace SampleWebApp.Test.TestUtility;

public class SamplePageTest : PageTest{
    [ClassDataSource<TestWebApplicationFactory>(Shared = SharedType.PerTestSession)]
    public required TestWebApplicationFactory Factory { get; init; }

    protected TestAuthenticationUsers GetTestAuthenticationUser() => this.Factory.GetTestAuthenticationUser();
}
