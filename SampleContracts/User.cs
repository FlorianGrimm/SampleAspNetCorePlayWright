namespace Sample.Contracts;

public sealed class User {
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string RowVersion { get; set; }
}