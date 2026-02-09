using System.Collections.Generic;
using UnityEngine;

public class Pawn : Unit
{
    public override List<Vector2Int> GetPossibleMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        Vector2Int startPos = BoardPosition;

        int forwardDirection = (team == Team.White) ? 1 : -1;
        int startRow = (team == Team.White) ? 1 : 6;


        Vector2Int oneForward = new Vector2Int(startPos.x, startPos.y + forwardDirection);
        if (IsWithinBoard(oneForward))
        {
            Cell forwardCell = BattleController.Instance?.GetCell(oneForward.x, oneForward.y);
            if (forwardCell != null && forwardCell.CurrentUnit == null)
            {
                moves.Add(oneForward);

                if (startPos.y == startRow)
                {
                    Vector2Int twoForward = new Vector2Int(startPos.x, startPos.y + 2 * forwardDirection);
                    Cell twoForwardCell = BattleController.Instance?.GetCell(twoForward.x, twoForward.y);
                    if (twoForwardCell != null && twoForwardCell.CurrentUnit == null)
                    {
                        moves.Add(twoForward);
                    }
                }
            }
        }

        Vector2Int[] attacks = {
            new Vector2Int(startPos.x + 1, startPos.y + forwardDirection),
            new Vector2Int(startPos.x - 1, startPos.y + forwardDirection)
        };

        foreach (Vector2Int attack in attacks)
        {
            if (!IsWithinBoard(attack))
                continue;

            Cell attackCell = BattleController.Instance?.GetCell(attack.x, attack.y);
            if (attackCell != null && attackCell.CurrentUnit != null &&
                attackCell.CurrentUnit.Team != team)
            {
                moves.Add(attack);
            }
        }

        return moves;
    }

    public override void MoveTo(Cell targetCell)
    {
        base.MoveTo(targetCell);

        int promotionRow = (team == Team.White) ? 7 : 0;
        if (BoardPosition.y == promotionRow)
        {
            BattleController.Instance?.RequestPawnPromotion(this);
        }
    }
}
