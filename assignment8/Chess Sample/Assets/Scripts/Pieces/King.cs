using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// King.cs
public class King : Piece
{
    public override MoveInfo[] GetMoves()
    {
        // --- TODO ---
        return new MoveInfo[]
        {
            new MoveInfo(1, 0, 1),    // ¡æ
            new MoveInfo(-1, 0, 1),   // ¡ç
            new MoveInfo(0, 1, 1),    // ¡è
            new MoveInfo(0, -1, 1),   // ¡é
            new MoveInfo(1, 1, 1),    // ¢Ö
            new MoveInfo(-1, 1, 1),   // ¢Ø
            new MoveInfo(-1, -1, 1),  // ¢×
            new MoveInfo(1, -1, 1)    // ¢Ù
        };
        // ------
    }
}
