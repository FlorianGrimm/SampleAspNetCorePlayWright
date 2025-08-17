using System.Security.Claims;

namespace SampleWebApp.Test.TestUtility.Test;

/// <summary>
/// Unit tests for the <see cref="TestAuthenticationUsers"/> class.
/// </summary>
public class TestAuthenticationUsersTest {
    
    [Test]
    public async Task GetUserClaims_ValidAdminCredentials_ReturnsCorrectClaims() {
        // Arrange
        var testUsers = new TestAuthenticationUsers();
        var user = testUsers.UserAdmin1;
        
        // Act
        var claims = testUsers.GetUserClaims(user.Name.AsSpan(), user.Password.AsSpan());

        // Assert
        await Assert.That(claims).IsNotNull();
        Debug.Assert(claims is not null);

        await Assert.That(claims!).HasCount().EqualTo(3);
        
        var nameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        await Assert.That(nameClaim).IsNotNull();
        await Assert.That(nameClaim!.Value).IsEqualTo(user.Name);
        
        var fullNameClaim = claims.FirstOrDefault(c => c.Type == "FullName");
        await Assert.That(fullNameClaim).IsNotNull();
        await Assert.That(fullNameClaim!.Value).IsEqualTo(user.Name);
        
        var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        await Assert.That(roleClaim).IsNotNull();
        await Assert.That(roleClaim!.Value).IsEqualTo("Administrator");
    }

    [Test]
    public async Task GetUserClaims_ValidPlannerCredentials_ReturnsCorrectClaims() {
        // Arrange
        var testUsers = new TestAuthenticationUsers();
        var user = testUsers.UserPlanner1;
        
        // Act
        var claims = testUsers.GetUserClaims(user.Name.AsSpan(), user.Password.AsSpan());
        
        // Assert
        await Assert.That(claims).IsNotNull();
        Debug.Assert(claims is not null);

        await Assert.That(claims!).HasCount().EqualTo(3);
        
        var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        await Assert.That(roleClaim).IsNotNull();
        await Assert.That(roleClaim!.Value).IsEqualTo("Planner");
    }

    [Test]
    public async Task GetUserClaims_ValidTesterCredentials_ReturnsCorrectClaims() {
        // Arrange
        var testUsers = new TestAuthenticationUsers();
        var user = testUsers.UserTester1;
        
        // Act
        var claims = testUsers.GetUserClaims(user.Name.AsSpan(), user.Password.AsSpan());
        
        // Assert
        await Assert.That(claims).IsNotNull();
        Debug.Assert(claims is not null);
        await Assert.That(claims!).HasCount().EqualTo(3);
        
        var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        await Assert.That(roleClaim).IsNotNull();
        await Assert.That(roleClaim!.Value).IsEqualTo("Tester");
    }

    [Test]
    public async Task GetUserClaims_ValidCustomerCredentials_ReturnsCorrectClaims() {
        // Arrange
        var testUsers = new TestAuthenticationUsers();
        var user = testUsers.UserCustomer1;
        
        // Act
        var claims = testUsers.GetUserClaims(user.Name.AsSpan(), user.Password.AsSpan());
        
        // Assert
        await Assert.That(claims).IsNotNull();
        Debug.Assert(claims is not null);
        await Assert.That(claims!).HasCount().EqualTo(3);
        
        var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        await Assert.That(roleClaim).IsNotNull();
        await Assert.That(roleClaim!.Value).IsEqualTo("Customer");
    }

    [Test]
    public async Task GetUserClaims_InvalidUsername_ReturnsNull() {
        // Arrange
        var testUsers = new TestAuthenticationUsers();
        
        // Act
        var claims = testUsers.GetUserClaims("invaliduser".AsSpan(), "anypassword".AsSpan());
        
        // Assert
        await Assert.That(claims).IsNull();
    }

    [Test]
    public async Task GetUserClaims_ValidUsernameInvalidPassword_ReturnsNull() {
        // Arrange
        var testUsers = new TestAuthenticationUsers();
        var user = testUsers.UserAdmin1;
        
        // Act
        var claims = testUsers.GetUserClaims(user.Name.AsSpan(), "wrongpassword".AsSpan());
        
        // Assert
        await Assert.That(claims).IsNull();
    }

    [Test]
    public async Task GetUserClaims_EmptyCredentials_ReturnsNull() {
        // Arrange
        var testUsers = new TestAuthenticationUsers();
        
        // Act
        var claims = testUsers.GetUserClaims("".AsSpan(), "".AsSpan());
        
        // Assert
        await Assert.That(claims).IsNull();
    }

    [Test]
    public async Task GetUserClaims_CaseSensitiveUsername_ReturnsNull() {
        // Arrange
        var testUsers = new TestAuthenticationUsers();
        var user = testUsers.UserAdmin1;
        
        // Act
        var claims = testUsers.GetUserClaims(user.Name.ToUpper().AsSpan(), user.Password.AsSpan());
        
        // Assert
        await Assert.That(claims).IsNull();
    }

    [Test]
    public async Task GetUserClaims_CaseSensitivePassword_ReturnsNull() {
        // Arrange
        var testUsers = new TestAuthenticationUsers();
        var user = testUsers.UserAdmin1;
        
        // Act
        var claims = testUsers.GetUserClaims(user.Name.AsSpan(), user.Password.ToUpper().AsSpan());
        
        // Assert
        await Assert.That(claims).IsNull();
    }

    [Test]
    public async Task GetUserClaims_AllPredefinedUsers_HaveUniqueNames() {
        // Arrange
        var testUsers = new TestAuthenticationUsers();
        var allUsers = new[] {
            testUsers.UserAdmin1, testUsers.UserAdmin2,
            testUsers.UserPlanner1, testUsers.UserPlanner2,
            testUsers.UserTester1, testUsers.UserTester2,
            testUsers.UserCustomer1, testUsers.UserCustomer2
        };
        
        // Act & Assert
        var userNames = allUsers.Select(u => u.Name).ToList();
        var uniqueNames = userNames.Distinct().ToList();
        
        await Assert.That(userNames).HasCount().EqualTo(uniqueNames.Count);
    }

    [Test]
    public async Task GetUserClaims_AllPredefinedUsers_HaveUniquePasswords() {
        // Arrange
        var testUsers = new TestAuthenticationUsers();
        var allUsers = new[] {
            testUsers.UserAdmin1, testUsers.UserAdmin2,
            testUsers.UserPlanner1, testUsers.UserPlanner2,
            testUsers.UserTester1, testUsers.UserTester2,
            testUsers.UserCustomer1, testUsers.UserCustomer2
        };
        
        // Act & Assert
        var passwords = allUsers.Select(u => u.Password).ToList();
        var uniquePasswords = passwords.Distinct().ToList();
        
        await Assert.That(passwords).HasCount().EqualTo(uniquePasswords.Count);
    }

    [Test]
    public async Task GetUserClaims_AllPredefinedUsers_CanAuthenticate() {
        // Arrange
        var testUsers = new TestAuthenticationUsers();
        var allUsers = new[] {
            testUsers.UserAdmin1, testUsers.UserAdmin2,
            testUsers.UserPlanner1, testUsers.UserPlanner2,
            testUsers.UserTester1, testUsers.UserTester2,
            testUsers.UserCustomer1, testUsers.UserCustomer2
        };
        
        // Act & Assert
        foreach (var user in allUsers) {
            var claims = testUsers.GetUserClaims(user.Name.AsSpan(), user.Password.AsSpan());
            
            await Assert.That(claims).IsNotNull();
            Debug.Assert(claims is not null);
            await Assert.That(claims!).HasCount().EqualTo(3);
            
            var nameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            await Assert.That(nameClaim).IsNotNull();
            await Assert.That(nameClaim!.Value).IsEqualTo(user.Name);
            
            var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            await Assert.That(roleClaim).IsNotNull();
            await Assert.That(roleClaim!.Value).IsEqualTo(user.Role);
        }
    }

    [Test]
    public async Task UserProperties_AllUsers_HaveExpectedRoles() {
        // Arrange
        var testUsers = new TestAuthenticationUsers();
        
        // Act & Assert
        await Assert.That(testUsers.UserAdmin1.Role).IsEqualTo("Administrator");
        await Assert.That(testUsers.UserAdmin2.Role).IsEqualTo("Administrator");
        await Assert.That(testUsers.UserPlanner1.Role).IsEqualTo("Planner");
        await Assert.That(testUsers.UserPlanner2.Role).IsEqualTo("Planner");
        await Assert.That(testUsers.UserTester1.Role).IsEqualTo("Tester");
        await Assert.That(testUsers.UserTester2.Role).IsEqualTo("Tester");
        await Assert.That(testUsers.UserCustomer1.Role).IsEqualTo("Customer");
        await Assert.That(testUsers.UserCustomer2.Role).IsEqualTo("Customer");
    }

    [Test]
    public async Task UserProperties_AllUsers_HaveNonEmptyNames() {
        // Arrange
        var testUsers = new TestAuthenticationUsers();
        var allUsers = new[] {
            testUsers.UserAdmin1, testUsers.UserAdmin2,
            testUsers.UserPlanner1, testUsers.UserPlanner2,
            testUsers.UserTester1, testUsers.UserTester2,
            testUsers.UserCustomer1, testUsers.UserCustomer2
        };
        
        // Act & Assert
        foreach (var user in allUsers) {
            await Assert.That(user.Name).IsNotNull();
            await Assert.That(user.Name).IsNotEmpty();
            await Assert.That(user.Password).IsNotNull();
            await Assert.That(user.Password).IsNotEmpty();
            await Assert.That(user.Role).IsNotNull();
            await Assert.That(user.Role).IsNotEmpty();
        }
    }
}
