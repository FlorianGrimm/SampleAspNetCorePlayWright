namespace Sample.BuisnessLogic.Model;

public sealed class MUser {
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required MRowVersion RowVersion { get; set; }
}

public sealed class MUserId {
    public required Guid Id { get; set; }
}

public sealed class MUserIdVersion {
    public required Guid Id { get; set; }
    public required MRowVersion RowVersion { get; set; }
}


public class ListWith<T> { 
    public required List<T> Items { get; set; }
    public required int Count { get; set; }
    public required MRowVersion RowVersion { get; set; }
}