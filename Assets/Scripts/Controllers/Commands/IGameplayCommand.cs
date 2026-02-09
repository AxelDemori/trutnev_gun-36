public interface IGameplayCommand
{
    void Execute();
    void Undo();
}