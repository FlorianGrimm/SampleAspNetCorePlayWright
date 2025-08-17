namespace SampleWebApp.Test.TestUtility;

/// <summary>
/// Authentication scheme options for the test authentication handler.
/// Extends the base <see cref="AuthenticationSchemeOptions"/> without additional configuration.
/// </summary>
public class TestAuthenticationSchemeOptions : AuthenticationSchemeOptions {
}

/// <summary>
/// Custom authentication handler for testing purposes that validates users against a predefined set of test users.
/// This handler parses custom authorization headers in the format "TestScheme username/password" and authenticates
/// users by looking them up in the <see cref="TestAuthenticationUsers"/> service.
/// </summary>
public class TestAuthHandler : AuthenticationHandler<TestAuthenticationSchemeOptions> {
    /// <summary>
    /// The authentication scheme name used by this handler.
    /// </summary>
    public static string AuthenticationScheme = "TestScheme";

    private TestAuthenticationUsers? _TestAuthenticationUser;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestAuthHandler"/> class.
    /// </summary>
    /// <param name="options">The authentication scheme options monitor.</param>
    /// <param name="logger">The logger factory for creating loggers.</param>
    /// <param name="encoder">The URL encoder for encoding URLs.</param>
    public TestAuthHandler(
        IOptionsMonitor<TestAuthenticationSchemeOptions> options,
        Microsoft.Extensions.Logging.ILoggerFactory logger,
        System.Text.Encodings.Web.UrlEncoder encoder)
        : base(options, logger, encoder) {
    }

    /// <summary>
    /// Handles the authentication process by parsing the Authorization header and validating credentials
    /// against the test user database.
    /// </summary>
    /// <returns>
    /// An <see cref="AuthenticateResult"/> indicating the result of the authentication attempt:
    /// - Success with claims if valid credentials are provided
    /// - Failure if invalid credentials are provided
    /// - NoResult if no authorization header is present or malformed
    /// </returns>
    protected override Task<AuthenticateResult> HandleAuthenticateAsync() {
        var testAuthenticationUser
            = this._TestAuthenticationUser
                ??= this.Context.RequestServices.GetRequiredService<TestAuthenticationUsers>();

        if (this.Context.Request.Headers.Authorization is { Count: > 0 } authorization) {
            if (authorization[0] is { Length: > 0 } authorizationValue) {
                if (TryParseAuthorizationHeader(authorizationValue, out var userName, out var password)) {
                    if (testAuthenticationUser.GetUserClaims(userName, password) is { Count: > 0} claims) {
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
        return Task.FromResult(AuthenticateResult.NoResult());
    }

    /// <summary>
    /// Attempts to parse an authorization header value in the format "TestScheme username/password".
    /// </summary>
    /// <param name="authorizationValue">The authorization header value to parse.</param>
    /// <param name="userName">When this method returns, contains the username if parsing succeeded; otherwise, an empty span.</param>
    /// <param name="password">When this method returns, contains the password if parsing succeeded; otherwise, an empty span.</param>
    /// <returns>
    /// <c>true</c> if the authorization header was successfully parsed and contains valid username/password format;
    /// otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// The expected format is: "TestScheme username/password" where:
    /// - The scheme must match <see cref="AuthenticationScheme"/>
    /// - There must be a space after the scheme name
    /// - The username and password must be separated by a forward slash (/)
    /// </remarks>
    public static bool TryParseAuthorizationHeader(string authorizationValue, out ReadOnlySpan<char> userName, out ReadOnlySpan<char> password) {
        var authorizationSpan = authorizationValue.AsSpan();
        if (authorizationSpan.StartsWith(TestAuthHandler.AuthenticationScheme.AsSpan(), StringComparison.Ordinal)) {
            authorizationSpan = authorizationSpan[TestAuthHandler.AuthenticationScheme.Length..^0];
            if (authorizationSpan.StartsWith(' ')) {
                authorizationSpan = authorizationSpan.TrimStart(' ');
                var pos = authorizationSpan.IndexOf('/');
                if (0 < pos) {
                    userName = authorizationSpan.Slice(0, pos);
                    password = authorizationSpan.Slice(pos + 1);
                    return true;
                }
            }
        }
        userName = password = string.Empty.AsSpan();
        return false;
    }
}

