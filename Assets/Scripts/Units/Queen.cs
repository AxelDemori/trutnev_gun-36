using System.Collections.Generic;
using UnityEngine;

public class Queen : Unit
{
    private Vector2Int[] directions = {
        new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0),
        new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1), new Vector2Int(-1, 1)
    };

    public override List<Vector2Int> GetPossibleMoves()
    {
        return GetSlidingMoves(directions);
    }
}