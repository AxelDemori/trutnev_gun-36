using System.Collections.Generic;
using UnityEngine;

public class Knight : Unit
{
    private Vector2Int[] knightMoves = {
        new Vector2Int(2, 1), new Vector2Int(2, -1),
        new Vector2Int(-2, 1), new Vector2Int(-2, -1),
        new Vector2Int(1, 2), new Vector2Int(1, -2),
        new Vector2Int(-1, 2), new Vector2Int(-1, -2)
    };

    public override List<Vector2Int> GetPossibleMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        Vector2Int startPos = BoardPosition;

        foreach (Vector2Int move in knightMoves)
        {
            Vector2Int nextPos = new Vector2Int(
                startPos.x + move.x,
                startPos.y + move.y
            );

            if (!IsWithinBoard(nextPos))
                continue;

            Cell targetCell = BattleController.Instance?.GetCell(nextPos.x, nextPos.y);
            if (targetCell == null)
                continue;

            
            if (targetCell.CurrentUnit == null || targetCell.CurrentUnit.Team != team)
            {
                moves.Add(nextPos);
            }
        }

        return moves;
    }
}
