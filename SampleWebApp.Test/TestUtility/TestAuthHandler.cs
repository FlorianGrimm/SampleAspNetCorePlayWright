using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace SampleWebApp.Test.TestUtility;

public class TestAuthenticationUsers {
    public TestUser UserAdmin1 { get; set; } = new TestUser("TestUserAdmin1", $"Admin1-{Guid.NewGuid()}", "Administrator");
    public TestUser UserAdmin2 { get; set; } = new TestUser("TestUserAdmin2", $"Admin2-{Guid.NewGuid()}", "Administrator");

    public TestUser UserPlanner1 { get; set; } = new TestUser("TestUserPlanner1", $"Planner1-{Guid.NewGuid()}", "Planner");
    public TestUser UserPlanner2 { get; set; } = new TestUser("TestUserPlanner2", $"Planner2-{Guid.NewGuid()}", "Planner");

    public TestUser UserTester1 { get; set; } = new TestUser("TestUserTester1", $"Tester1-{Guid.NewGuid()}", "Tester");
    public TestUser UserTester2 { get; set; } = new TestUser("TestUserTester2", $"Tester2-{Guid.NewGuid()}", "Tester");

    public TestUser UserCustomer1 { get; set; } = new TestUser("TestUserCustomer1", $"Customer1-{Guid.NewGuid()}", "Customer");
    public TestUser UserCustomer2 { get; set; } = new TestUser("TestUserCustomer2", $"Customer2-{Guid.NewGuid()}", "Customer");



    private List<TestUser>? _ListUser = null;

    public List<Claim>? GetUserClaims(ReadOnlySpan<char> userName, ReadOnlySpan<char> password) {
        var listUser = this._ListUser ??= new List<TestUser>() {
                this.UserAdmin1, this.UserAdmin2,
                this.UserPlanner1, this.UserPlanner2,
                this.UserTester1, this.UserTester2,
                this.UserCustomer1, this.UserCustomer2
        };

        foreach (var user in listUser) {
            if (userName.SequenceEqual(user.Name.AsSpan())) {
                if (password.SequenceEqual(user.Password.AsSpan())) {
                    return new List<Claim>
                    {
                        new Claim(System.Security.Claims.ClaimTypes.Name, user.Name),
                        new Claim("FullName", user.Name),
                        new Claim(ClaimTypes.Role, user.Role),
                    };
                }
            }
        }
        return null;
    }
}
public record TestUser(string Name, string Password, string Role) {
    public AuthenticationHeaderValue GetAuthenticationHeaderValue()
        => new System.Net.Http.Headers.AuthenticationHeaderValue(
            scheme: TestAuthHandler.AuthenticationScheme,
            parameter: $"{this.Name}/{this.Password}");
}
public class TestAuthenticationSchemeOptions : AuthenticationSchemeOptions {
}

public class TestAuthHandler : AuthenticationHandler<TestAuthenticationSchemeOptions> {
    public static string AuthenticationScheme = "TestScheme";

    private TestAuthenticationUsers? _TestAuthenticationUser;

    public TestAuthHandler(
        IOptionsMonitor<TestAuthenticationSchemeOptions> options,
        Microsoft.Extensions.Logging.ILoggerFactory logger,
        System.Text.Encodings.Web.UrlEncoder encoder)
        : base(options, logger, encoder) {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync() {
        var testAuthenticationUser
            = this._TestAuthenticationUser
                ??= this.Context.RequestServices.GetRequiredService<TestAuthenticationUsers>();

        if (this.Context.Request.Headers.Authorization is { Count: > 0 } authorization
            && authorization[0] is { Length: > 0 } authorizationValue) {

            var authorizationSpan = authorizationValue.AsSpan();
            if (authorizationSpan.StartsWith(TestAuthHandler.AuthenticationScheme.AsSpan(), StringComparison.Ordinal)) {
                authorizationSpan = authorizationSpan[TestAuthHandler.AuthenticationScheme.Length..^0];
                if (authorizationSpan.StartsWith(' ')) {
                    authorizationSpan = authorizationSpan.TrimStart(' ');
                    var pos = authorizationSpan.IndexOf('/');
                    if (0 < pos) {
                        var userName = authorizationSpan.Slice(0, pos);
                        var password = authorizationSpan.Slice(pos + 1);
                        if (testAuthenticationUser.GetUserClaims(userName, password) is { } claims) {
                            var identity = new ClaimsIdentity(claims, AuthenticationScheme);
                            var principal = new ClaimsPrincipal(identity);
                            var ticket = new AuthenticationTicket(principal, AuthenticationScheme);
                            return Task.FromResult(AuthenticateResult.Success(ticket));
                        } else {
                            return Task.FromResult(AuthenticateResult.Fail("unknown user"));
                        }
                    }
                }
            }
        }
        return Task.FromResult(AuthenticateResult.NoResult());
    }
}

