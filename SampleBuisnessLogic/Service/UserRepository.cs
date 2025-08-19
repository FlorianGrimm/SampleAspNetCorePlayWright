namespace Sample.BuisnessLogic.Service;

public class UserRepository {
    private UserRepositoryState _State = new UserRepositoryState();

    public UserRepositoryState State { get => this._State; set => this._State = value; }

    public UserRepository() {

    }
    public UserRepository(UserRepositoryState state) {
        this.State = state;
    }
    public UserRepositoryState CreateUser(MUser value) {
        var action = new CreateUser() { Value = value };
        while (true) {
            var state = this.State;
            var nextState = this.ApplyCreateUser(state, action);

            var actions = state.Actions.Add(new CreateUser() { Value = value });
            UserRepositoryState result = nextState with { Actions = actions };
            if (ReferenceEquals(
                System.Threading.Interlocked.CompareExchange(ref this._State, result, state),
                state)) { return result; }
        }
    }

    public UserRepositoryState Execute(ITransformState<MUser> action) {
        while (true) {
            var state = this.State;
            var nextState = this.Apply(state, action);
            var actions = state.Actions.Add(action);
            UserRepositoryState result = nextState with { Actions = actions };
            if (ReferenceEquals(
                System.Threading.Interlocked.CompareExchange(ref this._State, result, state),
                state)) { return result; }
        }
    }

    private UserRepositoryState ApplyCreateUser(UserRepositoryState state, CreateUser action) {
        var value = action.Value;
        var items = state.Items.Add(value.Id, value);
        return state with { Items = items };
    }

    public UserRepositoryState Apply(UserRepositoryState state, ITransformState<MUser> action) {
        if (action is CreateUser createUser) {
            return this.ApplyCreateUser(state, createUser);
        } else {
            throw new ArgumentException(nameof(action));
        }
    }
}

public record UserRepositoryState(
    ImmutableDictionary<Guid, MUser> Items,
    ImmutableArray<ITransformState<MUser>> Actions
    ) {
    public UserRepositoryState()
        : this(
            ImmutableDictionary<Guid, MUser>.Empty,
            ImmutableArray<ITransformState<MUser>>.Empty
        ) {
    }
}

public interface ITransformState<T> {
    public string GetOperation();
}

public class CreateUser : ITransformState<MUser> {
    public string GetOperation() => "User.Create";
    public required MUser Value { get; set; }
}

public class UpdateUser : ITransformState<MUser> {
    public string GetOperation() => "User.Update";
    public required MUser Value { get; set; }
}


public class DeleteUser : ITransformState<MUser> {
    public string GetOperation() => "User.Delete";
    public required MUser Value { get; set; }
}

