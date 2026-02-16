using System.Collections.Generic;
using UnityEngine;

public class Rook : Unit
{
    private Vector2Int[] directions = {
        new Vector2Int(0, 1), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(-1, 0)
    };

    public override List<Vector2Int> GetPossibleMoves()
    {
        return GetSlidingMoves(directions);
    }
}