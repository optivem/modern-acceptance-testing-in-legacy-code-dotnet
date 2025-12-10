namespace Optivem.Testing.Dsl;

public interface ICommand<T>
{
    T Execute();
}
