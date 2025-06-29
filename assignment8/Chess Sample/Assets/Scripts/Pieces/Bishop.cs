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
        new MoveInfo(1, 1, 0),    // ¢Ö
        new MoveInfo(-1, 1, 0),   // ¢Ø
        new MoveInfo(-1, -1, 0),  // ¢×
        new MoveInfo(1, -1, 0)    // ¢Ù
        };
        // ------
    }
}