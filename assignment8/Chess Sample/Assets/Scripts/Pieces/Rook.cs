using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rook.cs
public class Rook : Piece
{
    public override MoveInfo[] GetMoves()
    {
        // --- TODO ---
        return new MoveInfo[]
        {
        new MoveInfo(1, 0, 7),   // 오른쪽
        new MoveInfo(-1, 0, 7),  // 왼쪽
        new MoveInfo(0, 1, 7),   // 위쪽
        new MoveInfo(0, -1, 7)   // 아래쪽
        };
        // ------
    }
}
