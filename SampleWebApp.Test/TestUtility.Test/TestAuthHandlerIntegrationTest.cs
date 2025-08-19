using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Sample.WebApp.Test.TestUtility.Test;

/// <summary>
/// Integration tests for the <see cref="TestAuthHandler"/> authentication process.
/// </summary>
public class TestAuthHandlerIntegrationTest {

    /// <summary>
    /// Creates a test authentication handler with all required dependencies.
    /// </summary>
    /// <returns>A configured <see cref="TestAuthHandler"/> instance.</returns>
    private static TestAuthHandler CreateTestAuthHandler(out HttpContext httpContext, out TestAuthenticationUsers testUsers) {
        // Create service collection and register dependencies
        var services = new ServiceCollection();
        testUsers = new TestAuthenticationUsers();
        services.AddSingleton(testUsers);
        services.AddLogging();

        var serviceProvider = services.BuildServiceProvider();

        // Create HTTP context
        httpContext = new DefaultHttpContext();
        httpContext.RequestServices = serviceProvider;

        // Create options monitor
        var options = new TestAuthenticationSchemeOptions();
        var optionsMonitor = new OptionsMonitorWrapper<TestAuthenticationSchemeOptions>(options);

        // Create logger factory
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

        // Create URL encoder
        var urlEncoder = UrlEncoder.Default;

        // Create and initialize the handler
        var handler = new TestAuthHandler(optionsMonitor, loggerFactory, urlEncoder);

        // Initialize the handler with the context
        var scheme = new AuthenticationScheme(TestAuthHandler.AuthenticationScheme, null, typeof(TestAuthHandler));
        handler.InitializeAsync(scheme, httpContext).Wait();

        return handler;
    }

    [Test]
    public async Task HandleAuthenticateAsync_NoAuthorizationHeader_ReturnsNoResult() {
        // Arrange
        var handler = CreateTestAuthHandler(out var httpContext, out var testUsers);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        await Assert.That(result.Succeeded).IsEqualTo(false);
        await Assert.That(result.None).IsEqualTo(true);
        await Assert.That(result.Principal).IsNull();
    }

    [Test]
    public async Task HandleAuthenticateAsync_EmptyAuthorizationHeader_ReturnsNoResult() {
        // Arrange
        var handler = CreateTestAuthHandler(out var httpContext, out var testUsers);
        httpContext.Request.Headers.Authorization = "";

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        await Assert.That(result.Succeeded).IsEqualTo(false);
        await Assert.That(result.None).IsEqualTo(true);
        await Assert.That(result.Principal).IsNull();
    }

    [Test]
    public async Task HandleAuthenticateAsync_InvalidAuthorizationFormat_ReturnsNoResult() {
        // Arrange
        var handler = CreateTestAuthHandler(out var httpContext, out var testUsers);
        httpContext.Request.Headers.Authorization = "Bearer token123";

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        await Assert.That(result.Succeeded).IsEqualTo(false);
        await Assert.That(result.None).IsEqualTo(true);
        await Assert.That(result.Principal).IsNull();
    }

    [Test]
    public async Task HandleAuthenticateAsync_ValidCredentials_ReturnsSuccess() {
        // Arrange
        var handler = CreateTestAuthHandler(out var httpContext, out var testUsers);
        var user = testUsers.UserAdmin1;
        httpContext.Request.Headers.Authorization = $"TestScheme {user.Name}/{user.Password}";

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        await Assert.That(result.Succeeded).IsEqualTo(true);
        await Assert.That(result.Principal).IsNotNull();
        await Assert.That(result.Principal!.Identity!.IsAuthenticated).IsEqualTo(true);
        await Assert.That(result.Principal.Identity.Name).IsEqualTo(user.Name);

        // Check claims
        var claims = result.Principal.Claims.ToList();
        await Assert.That(claims).HasCount().EqualTo(3);

        var nameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        await Assert.That(nameClaim).IsNotNull();
        await Assert.That(nameClaim!.Value).IsEqualTo(user.Name);

        var fullNameClaim = claims.FirstOrDefault(c => c.Type == "FullName");
        await Assert.That(fullNameClaim).IsNotNull();
        await Assert.That(fullNameClaim!.Value).IsEqualTo(user.Name);

        var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        await Assert.That(roleClaim).IsNotNull();
        await Assert.That(roleClaim!.Value).IsEqualTo(user.Role);
    }

    [Test]
    public async Task HandleAuthenticateAsync_InvalidCredentials_ReturnsFailure() {
        // Arrange
        var handler = CreateTestAuthHandler(out var httpContext, out var testUsers);
        httpContext.Request.Headers.Authorization = "TestScheme invaliduser/invalidpass";

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        await Assert.That(result.Succeeded).IsEqualTo(false);
        await Assert.That(result.Failure).IsNotNull();
        await Assert.That(result.Failure!.Message).IsEqualTo("unknown user");
        await Assert.That(result.Principal).IsNull();
    }

    [Test]
    public async Task HandleAuthenticateAsync_ValidUsernameInvalidPassword_ReturnsFailure() {
        // Arrange
        var handler = CreateTestAuthHandler(out var httpContext, out var testUsers);
        var user = testUsers.UserPlanner1;
        httpContext.Request.Headers.Authorization = $"TestScheme {user.Name}/wrongpassword";

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        await Assert.That(result.Succeeded).IsEqualTo(false);
        await Assert.That(result.Failure).IsNotNull();
        await Assert.That(result.Failure!.Message).IsEqualTo("unknown user");
        await Assert.That(result.Principal).IsNull();
    }

    [Test]
    public async Task HandleAuthenticateAsync_DifferentUserRoles_ReturnsCorrectClaims() {
        for (int i = 0; i < 4; i++) {
            // Test different user types
            var handler = CreateTestAuthHandler(out var httpContext, out var testUsers);
            var user = (i) switch {
                0 => testUsers.UserAdmin1,
                1 => testUsers.UserPlanner1,
                2 => testUsers.UserTester1,
                3 => testUsers.UserCustomer1,
                _ => throw new Exception("should not happen")
            };
            // Reset the handler for each test case
            httpContext.Request.Headers.Authorization = $"TestScheme {user.Name}/{user.Password}";

            // Act
            var result = await handler.AuthenticateAsync();

            // Assert
            await Assert.That(result.Succeeded).IsEqualTo(true);
            await Assert.That(result.Principal).IsNotNull();

            var roleClaim = result.Principal!.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            await Assert.That(roleClaim).IsNotNull();
            await Assert.That(roleClaim!.Value).IsEqualTo(user.Role);
        }
    }
}
