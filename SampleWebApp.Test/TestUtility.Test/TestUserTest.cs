namespace Sample.WebApp.Test.TestUtility.Test;

/// <summary>
/// Unit tests for the <see cref="TestUser"/> record and <see cref="TestUserExtension"/> class.
/// </summary>
public class TestUserTest {
    
    [Test]
    public async Task GetAuthenticationHeaderValue_ValidUser_ReturnsCorrectHeader() {
        // Arrange
        var user = new TestUser("testuser", "testpass", "Admin");
        
        // Act
        var authHeader = user.GetAuthenticationHeaderValue();
        
        // Assert
        await Assert.That(authHeader).IsNotNull();
        await Assert.That(authHeader.Scheme).IsEqualTo(TestAuthHandler.AuthenticationScheme);
        await Assert.That(authHeader.Parameter).IsEqualTo("testuser/testpass");
    }

    [Test]
    public async Task GetAuthenticationHeaderValue_HeaderCanBeParsedByTestAuthHandler() {
        // Arrange
        var user = new TestUser("testuser", "testpass", "Admin");
        var authHeader = user.GetAuthenticationHeaderValue();
        var fullHeaderValue = $"{authHeader.Scheme} {authHeader.Parameter}";
        
        // Act
        var parseResult = TestAuthHandler.TryParseAuthorizationHeader(
            fullHeaderValue, out var userName, out var password);
        var userNameString = userName.ToString();
        var passwordString = password.ToString();

        // Assert
        await Assert.That(parseResult).IsEqualTo(true);
        await Assert.That(userNameString).IsEqualTo(user.Name);
        await Assert.That(passwordString).IsEqualTo(user.Password);
    }
}
