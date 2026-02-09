public class MoveCommand : IGameplayCommand
{
    private Unit unit;
    private Cell fromCell;
    private Cell toCell;
    private Unit capturedUnit;

    public MoveCommand(Unit unit, Cell targetCell)
    {
        this.unit = unit;
        this.fromCell = unit.CurrentCell;
        this.toCell = targetCell;
        this.capturedUnit = targetCell.CurrentUnit;
    }

    public void Execute()
    {
        if (capturedUnit != null)
        {
            capturedUnit.Capture();
        }

        unit.MoveTo(toCell);
    }

    public void Undo()
    {

        unit.MoveTo(fromCell);

    }
}