using System.Collections.Generic;
using UnityEngine;

public class King : Unit
{
    private Vector2Int[] allDirections = {
        new Vector2Int(1, 0), new Vector2Int(-1, 0),
        new Vector2Int(0, 1), new Vector2Int(0, -1),
        new Vector2Int(1, 1), new Vector2Int(1, -1),
        new Vector2Int(-1, 1), new Vector2Int(-1, -1)
    };

    public override List<Vector2Int> GetPossibleMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        Vector2Int startPos = BoardPosition;

        foreach (Vector2Int dir in allDirections)
        {
            Vector2Int nextPos = new Vector2Int(
                startPos.x + dir.x,
                startPos.y + dir.y
            );

            if (!IsWithinBoard(nextPos))
                continue;

            Cell targetCell = BattleController.Instance.GetCell(nextPos.x, nextPos.y);
            if (targetCell == null)
                continue;

            if (targetCell.CurrentUnit != null && targetCell.CurrentUnit.Team == team)
                continue;

            moves.Add(nextPos);
        }

        if (isFirstMove)
        {

            if (CanCastleKingside())
                moves.Add(new Vector2Int(6, startPos.y));

            if (CanCastleQueenside())
                moves.Add(new Vector2Int(2, startPos.y));
        }

        return moves;
    }

    private bool CanCastleKingside()
    {
        int row = (team == Team.White) ? 0 : 7;
        int kingX = 4;
        int rookX = 7;

        Cell rookCell = BattleController.Instance.GetCell(rookX, row);
        if (rookCell.CurrentUnit == null ||
            rookCell.CurrentUnit.PieceType != PieceType.Rook ||
            !rookCell.CurrentUnit.IsFirstMove)
        {
            return false;
        }

        for (int x = kingX + 1; x < rookX; x++)
        {
            Cell cell = BattleController.Instance.GetCell(x, row);
            if (cell.CurrentUnit != null)
                return false;
        }

        return true;
    }

    private bool CanCastleQueenside()
    {
        int row = (team == Team.White) ? 0 : 7;
        int kingX = 4;
        int rookX = 0;

        Cell rookCell = BattleController.Instance.GetCell(rookX, row);
        if (rookCell.CurrentUnit == null ||
            rookCell.CurrentUnit.PieceType != PieceType.Rook ||
            !rookCell.CurrentUnit.IsFirstMove)
        {
            return false;
        }

        for (int x = kingX - 1; x > rookX; x--)
        {
            Cell cell = BattleController.Instance.GetCell(x, row);
            if (cell.CurrentUnit != null)
                return false;
        }

        return true;
    }
}