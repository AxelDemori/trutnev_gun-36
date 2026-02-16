using System.Collections.Generic;
using UnityEngine;

public class Pawn : Unit
{
    private Vector2Int enPassantTarget = new Vector2Int(-1, -1);

    public override List<Vector2Int> GetPossibleMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        Vector2Int startPos = BoardPosition;

        int direction = (team == Team.White) ? 1 : -1;
        int startRow = (team == Team.White) ? 1 : 6;
        int promotionRow = (team == Team.White) ? 7 : 0;
        Vector2Int oneForward = new Vector2Int(startPos.x, startPos.y + direction);

        if (IsWithinBoard(oneForward))
        {
            Cell forwardCell = BattleController.Instance.GetCell(oneForward.x, oneForward.y);
            if (forwardCell != null && forwardCell.CurrentUnit == null)
            {
                moves.Add(oneForward);

                if (startPos.y == startRow)
                {
                    Vector2Int twoForward = new Vector2Int(startPos.x, startPos.y + 2 * direction);
                    Cell twoForwardCell = BattleController.Instance.GetCell(twoForward.x, twoForward.y);
                    if (twoForwardCell != null && twoForwardCell.CurrentUnit == null)
                    {
                        moves.Add(twoForward);
                    }
                }
            }
        }

        Vector2Int[] attacks = {
            new Vector2Int(startPos.x + 1, startPos.y + direction),
            new Vector2Int(startPos.x - 1, startPos.y + direction)
        };

        foreach (Vector2Int attack in attacks)
        {
            if (!IsWithinBoard(attack))
                continue;

            Cell attackCell = BattleController.Instance.GetCell(attack.x, attack.y);
            if (attackCell != null && attackCell.CurrentUnit != null &&
                attackCell.CurrentUnit.Team != team)
            {
                moves.Add(attack);
            }
        }

        if (startPos.y == (team == Team.White ? 4 : 3))
        {
            Vector2Int[] enPassantTargets = {
                new Vector2Int(startPos.x + 1, startPos.y),
                new Vector2Int(startPos.x - 1, startPos.y)
            };

            foreach (Vector2Int enemyPos in enPassantTargets)
            {
                if (!IsWithinBoard(enemyPos))
                    continue;

                Cell enemyCell = BattleController.Instance.GetCell(enemyPos.x, enemyPos.y);

                if (enemyCell != null && enemyCell.CurrentUnit != null &&
                    enemyCell.CurrentUnit.Team != team &&
                    enemyCell.CurrentUnit.PieceType == PieceType.Pawn)
                {
                    if (enemyCell.CurrentUnit is Pawn enemyPawn && enemyPawn.JustMovedTwoForward)
                    {
                        Vector2Int enPassantMove = new Vector2Int(enemyPos.x, startPos.y + direction);
                        if (IsWithinBoard(enPassantMove))
                        {
                            Cell enPassantCell = BattleController.Instance.GetCell(enPassantMove.x, enPassantMove.y);
                            if (enPassantCell != null && enPassantCell.CurrentUnit == null)
                            {
                                moves.Add(enPassantMove);
                                enPassantTarget = enPassantMove;
                            }
                        }
                    }
                }
            }
        }

        return moves;
    }

    public override void MoveTo(Cell targetCell)
    {
        Vector2Int startPos = BoardPosition;
        Vector2Int targetPos = targetCell.BoardPosition;
        int direction = (team == Team.White) ? 1 : -1;
        bool isEnPassant = (Mathf.Abs(targetPos.x - startPos.x) == 1 &&
                           targetPos.y == startPos.y + direction &&
                           targetCell.CurrentUnit == null);

        if (isEnPassant)
        {
            Vector2Int enemyPawnPos = new Vector2Int(targetPos.x, startPos.y);
            Cell enemyCell = BattleController.Instance.GetCell(enemyPawnPos.x, enemyPawnPos.y);

            if (enemyCell != null && enemyCell.CurrentUnit != null &&
                enemyCell.CurrentUnit.Team != team &&
                enemyCell.CurrentUnit.PieceType == PieceType.Pawn)
            {
                Unit enemyPawn = enemyCell.CurrentUnit;
                enemyPawn.Capture();
                BattleController.Instance.RemoveUnit(enemyPawn);
            }
        }

        base.MoveTo(targetCell);

        if (Mathf.Abs(targetPos.y - startPos.y) == 2)
        {
            JustMovedTwoForward = true;
            BattleController.Instance.SetEnPassantTarget(this, targetPos);
        }
        int promotionRow = (team == Team.White) ? 7 : 0;
        if (targetPos.y == promotionRow)
        {
            BattleController.Instance.RequestPawnPromotion(this);
        }
    }
    public bool JustMovedTwoForward { get; private set; }
    public void ResetJustMovedTwoForward()
    {
        JustMovedTwoForward = false;
    }
}