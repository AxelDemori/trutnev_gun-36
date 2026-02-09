using System.Collections.Generic;
using UnityEngine;

public class Bishop : Unit
{
    public override List<Vector2Int> GetPossibleMoves()
    {
        return GetSlidingMoves(bishopDirections);
    }
}