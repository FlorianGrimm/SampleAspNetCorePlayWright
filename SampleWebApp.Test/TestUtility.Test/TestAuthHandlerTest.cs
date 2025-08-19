namespace Sample.WebApp.Test.TestUtility.Test;

/// <summary>
/// Unit tests for the <see cref="TestAuthHandler"/> class.
/// </summary>
public class TestAuthHandlerTest {

    [Test]
    public async Task TryParseAuthorizationHeader_EmptyString_ReturnsFalse() {
        // Act
        var result = TestAuthHandler.TryParseAuthorizationHeader("", out var userName, out var password);
        var userNameIsEmpty = userName.IsEmpty;
        var passwordIsEmpty = password.IsEmpty;

        // Assert
        await Assert.That(result).IsEqualTo(false);
        await Assert.That(userNameIsEmpty).IsEqualTo(true);
        await Assert.That(passwordIsEmpty).IsEqualTo(true);
    }

    [Test]
    public async Task TryParseAuthorizationHeader_NullString_ReturnsFalse() {
        // Act
        var result = TestAuthHandler.TryParseAuthorizationHeader(null!, out var userName, out var password);
        var userNameIsEmpty = userName.IsEmpty;
        var passwordIsEmpty = password.IsEmpty;

        // Assert
        await Assert.That(result).IsEqualTo(false);
        await Assert.That(userNameIsEmpty).IsEqualTo(true);
        await Assert.That(passwordIsEmpty).IsEqualTo(true);
    }

    [Test]
    public async Task TryParseAuthorizationHeader_WrongScheme_ReturnsFalse() {
        // Arrange
        var authHeader = "Bearer token123";

        // Act
        var result = TestAuthHandler.TryParseAuthorizationHeader(authHeader, out var userName, out var password);
        var userNameIsEmpty = userName.IsEmpty;
        var passwordIsEmpty = password.IsEmpty;

        // Assert
        await Assert.That(result).IsEqualTo(false);
        await Assert.That(userNameIsEmpty).IsEqualTo(true);
        await Assert.That(passwordIsEmpty).IsEqualTo(true);
    }

    [Test]
    public async Task TryParseAuthorizationHeader_CorrectSchemeNoSpace_ReturnsFalse() {
        // Arrange
        var authHeader = "TestSchemeuser/pass";

        // Act
        var result = TestAuthHandler.TryParseAuthorizationHeader(authHeader, out var userName, out var password);
        var userNameIsEmpty = userName.IsEmpty;
        var passwordIsEmpty = password.IsEmpty;

        // Assert
        await Assert.That(result).IsEqualTo(false);
        await Assert.That(userNameIsEmpty).IsEqualTo(true);
        await Assert.That(passwordIsEmpty).IsEqualTo(true);
    }

    [Test]
    public async Task TryParseAuthorizationHeader_CorrectSchemeNoSlash_ReturnsFalse() {
        // Arrange
        var authHeader = "TestScheme userpass";

        // Act
        var result = TestAuthHandler.TryParseAuthorizationHeader(authHeader, out var userName, out var password);
        var userNameIsEmpty = userName.IsEmpty;
        var passwordIsEmpty = password.IsEmpty;

        // Assert
        await Assert.That(result).IsEqualTo(false);
        await Assert.That(userNameIsEmpty).IsEqualTo(true);
        await Assert.That(passwordIsEmpty).IsEqualTo(true);
    }

    [Test]
    public async Task TryParseAuthorizationHeader_ValidFormat_ReturnsTrue() {
        // Arrange
        var authHeader = "TestScheme testuser/testpass";

        // Act
        var result = TestAuthHandler.TryParseAuthorizationHeader(authHeader, out var userName, out var password);
        var userNameString = userName.ToString();
        var passwordString = password.ToString();

        // Assert
        await Assert.That(result).IsEqualTo(true);
        await Assert.That(userNameString).IsEqualTo("testuser");
        await Assert.That(passwordString).IsEqualTo("testpass");
    }

    [Test]
    public async Task TryParseAuthorizationHeader_ValidFormatWithMultipleSpaces_ReturnsTrue() {
        // Arrange
        var authHeader = "TestScheme   testuser/testpass";

        // Act
        var result = TestAuthHandler.TryParseAuthorizationHeader(authHeader, out var userName, out var password);
        var userNameString = userName.ToString();
        var passwordString = password.ToString();


        // Assert
        await Assert.That(result).IsEqualTo(true);
        await Assert.That(userNameString).IsEqualTo("testuser");
        await Assert.That(passwordString).IsEqualTo("testpass");
    }

    [Test]
    public async Task TryParseAuthorizationHeader_ValidFormatWithSpecialCharacters_ReturnsTrue() {
        // Arrange
        var authHeader = "TestScheme user@domain.com/P@ssw0rd!";

        // Act
        var result = TestAuthHandler.TryParseAuthorizationHeader(authHeader, out var userName, out var password);
        var userNameString = userName.ToString();
        var passwordString = password.ToString();

        // Assert
        await Assert.That(result).IsEqualTo(true);
        await Assert.That(userNameString).IsEqualTo("user@domain.com");
        await Assert.That(passwordString).IsEqualTo("P@ssw0rd!");
    }

    [Test]
    public async Task TryParseAuthorizationHeader_EmptyUsername_ReturnsFalse() {
        // Arrange
        var authHeader = "TestScheme /password";

        // Act
        var result = TestAuthHandler.TryParseAuthorizationHeader(authHeader, out var userName, out var password);
        var userNameIsEmpty = userName.IsEmpty;
        var passwordIsEmpty = password.IsEmpty;

        // Assert
        await Assert.That(result).IsEqualTo(false);
        await Assert.That(userNameIsEmpty).IsEqualTo(true);
        await Assert.That(passwordIsEmpty).IsEqualTo(true);
    }

    [Test]
    public async Task TryParseAuthorizationHeader_EmptyPassword_ReturnsTrue() {
        // Arrange
        var authHeader = "TestScheme username/";

        // Act
        var result = TestAuthHandler.TryParseAuthorizationHeader(authHeader, out var userName, out var password);
        var userNameString = userName.ToString();
        var passwordString = password.ToString();

        // Assert
        await Assert.That(result).IsEqualTo(true);
        await Assert.That(userNameString).IsEqualTo("username");
        await Assert.That(passwordString).IsEqualTo("");
    }
}
