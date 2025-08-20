#define Tracor
using System.Diagnostics;

namespace Sample.BuisnessLogic.Service;

public interface ITracor {
    void Trace<T>(TracorIdentitfier callee, T Value);
}

public class RuntimeTracor : ITracor {
    public void Trace<T>(TracorIdentitfier callee, T Value) {
    }

    //public void Trace<T>(TracorIdentitfier callee, T Value) where T : class {
    //}

    //public void Trace<T>(TracorIdentitfier callee, ref T Value) where T : struct {
    //}
}

public class TesttimeTracor : ITracor {
    [Conditional("Tracor")]
    public void Trace<T>(TracorIdentitfier callee, T Value) {
    }

    //public void Trace<T>(TracorIdentitfier callee, T Value) where T : class {
    //}

    //public void Trace<T>(TracorIdentitfier callee, ref T Value) where T : struct {
    //}
}

/*
 https://andrewlock.net/exploring-the-dotnet-8-preview-using-the-new-configuration-binder-source-generator/
https://github.com/martinothamar/Mediator
 */
public record class TracorIdentitfier (string Callee) {
    public TracorIdentitfier(TracorIdentitfier parent, string callee)
        : this(parent.Callee + callee){
    }
}