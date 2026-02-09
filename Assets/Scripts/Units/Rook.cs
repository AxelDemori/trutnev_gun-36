using System.Collections.Generic;
using UnityEngine;

public class Rook : Unit
{
    public override List<Vector2Int> GetPossibleMoves()
    {
        return GetSlidingMoves(rookDirections);
    }
}