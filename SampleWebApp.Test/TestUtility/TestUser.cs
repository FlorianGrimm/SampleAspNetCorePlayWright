namespace Sample.WebApp.Test.TestUtility;

/// <summary>
/// Represents a test user with authentication credentials and role information.
/// </summary>
/// <param name="Name">The username for authentication.</param>
/// <param name="Password">The password for authentication.</param>
/// <param name="Role">The role assigned to the user (e.g., Administrator, Planner, Tester, Customer).</param>
public record TestUser(string Name, string Password, string Role);

/// <summary>
/// Extension methods for the <see cref="TestUser"/> record.
/// </summary>
public static class TestUserExtension {

    /// <summary>
    /// Creates an <see cref="AuthenticationHeaderValue"/> for HTTP authentication using the test authentication scheme.
    /// </summary>
    /// <param name="that">The test user to create the authentication header for.</param>
    /// <returns>
    /// An <see cref="AuthenticationHeaderValue"/> with the scheme set to <see cref="TestAuthHandler.AuthenticationScheme"/>
    /// and the parameter formatted as "username/password".
    /// </returns>
    /// <remarks>
    /// The returned header value can be used with HTTP clients to authenticate requests
    /// using the custom test authentication scheme.
    /// </remarks>
    public static AuthenticationHeaderValue GetAuthenticationHeaderValue(this TestUser that)
        => new System.Net.Http.Headers.AuthenticationHeaderValue(
            scheme: TestAuthHandler.AuthenticationScheme,
            parameter: $"{that.Name}/{that.Password}");
}