using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override MoveInfo[] GetMoves()
    {
        // --- TODO ---
        return new MoveInfo[]
        {
            new MoveInfo(1, 1, 7),
            new MoveInfo(-1, 1, 7),
            new MoveInfo(1, -1, 7),
            new MoveInfo(-1, -1, 7),
        };
        // ------
    }
}