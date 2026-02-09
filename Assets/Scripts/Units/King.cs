using System.Collections.Generic;
using UnityEngine;

public class King : Unit
{
    private Vector2Int[] kingMoves = {
        new Vector2Int(1, 0), new Vector2Int(-1, 0),
        new Vector2Int(0, 1), new Vector2Int(0, -1),
        new Vector2Int(1, 1), new Vector2Int(1, -1),
        new Vector2Int(-1, 1), new Vector2Int(-1, -1)
    };

    public override List<Vector2Int> GetPossibleMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        Vector2Int startPos = BoardPosition;

        foreach (Vector2Int move in kingMoves)
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

        
        if (IsFirstMove)
        {

            if (CanCastle(true))
                moves.Add(new Vector2Int(6, startPos.y));


            if (CanCastle(false))
                moves.Add(new Vector2Int(2, startPos.y));
        }

        return moves;
    }

    private bool CanCastle(bool kingside)
    {

        return true;
    }
}