using System.Security.Claims;

namespace Sample.WebApp.Test.TestUtility;

/// <summary>
/// Provides a collection of predefined test users for authentication testing.
/// This class contains users with different roles (Administrator, Planner, Tester, Customer)
/// and provides methods to validate credentials and retrieve user claims.
/// </summary>
public class TestAuthenticationUsers {
    /// <summary>
    /// Gets the first test user with Administrator role.
    /// </summary>
    public TestUser UserAdmin1 { get; } = new TestUser("TestUserAdmin1", $"Admin1-{Guid.NewGuid()}", "Administrator");

    /// <summary>
    /// Gets the second test user with Administrator role.
    /// </summary>
    public TestUser UserAdmin2 { get; } = new TestUser("TestUserAdmin2", $"Admin2-{Guid.NewGuid()}", "Administrator");

    /// <summary>
    /// Gets the first test user with Planner role.
    /// </summary>
    public TestUser UserPlanner1 { get; } = new TestUser("TestUserPlanner1", $"Planner1-{Guid.NewGuid()}", "Planner");

    /// <summary>
    /// Gets the second test user with Planner role.
    /// </summary>
    public TestUser UserPlanner2 { get; } = new TestUser("TestUserPlanner2", $"Planner2-{Guid.NewGuid()}", "Planner");

    /// <summary>
    /// Gets the first test user with Tester role.
    /// </summary>
    public TestUser UserTester1 { get; } = new TestUser("TestUserTester1", $"Tester1-{Guid.NewGuid()}", "Tester");

    /// <summary>
    /// Gets the second test user with Tester role.
    /// </summary>
    public TestUser UserTester2 { get; } = new TestUser("TestUserTester2", $"Tester2-{Guid.NewGuid()}", "Tester");

    /// <summary>
    /// Gets the first test user with Customer role.
    /// </summary>
    public TestUser UserCustomer1 { get; } = new TestUser("TestUserCustomer1", $"Customer1-{Guid.NewGuid()}", "Customer");

    /// <summary>
    /// Gets the second test user with Customer role.
    /// </summary>
    public TestUser UserCustomer2 { get; } = new TestUser("TestUserCustomer2", $"Customer2-{Guid.NewGuid()}", "Customer");



    private List<TestUser>? _ListUser = null;

    /// <summary>
    /// Validates the provided credentials against the predefined test users and returns the associated claims.
    /// </summary>
    /// <param name="userName">The username to validate.</param>
    /// <param name="password">The password to validate.</param>
    /// <returns>
    /// A list of claims for the authenticated user if credentials are valid; otherwise, <c>null</c>.
    /// The returned claims include:
    /// - <see cref="ClaimTypes.Name"/>: The user's name
    /// - "FullName": The user's full name (same as name in this implementation)
    /// - <see cref="ClaimTypes.Role"/>: The user's role (Administrator, Planner, Tester, or Customer)
    /// </returns>
    /// <remarks>
    /// This method performs case-sensitive comparison of both username and password.
    /// All predefined users have unique usernames and passwords generated with GUIDs.
    /// </remarks>
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

