using System.Collections.Generic;
using UnityEngine;

public class Queen : Unit
{
    public override List<Vector2Int> GetPossibleMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        moves.AddRange(GetSlidingMoves(rookDirections));
        moves.AddRange(GetSlidingMoves(bishopDirections));
        return moves;
    }
}