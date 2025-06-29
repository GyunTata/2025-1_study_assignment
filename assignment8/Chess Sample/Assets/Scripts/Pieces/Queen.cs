using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public override MoveInfo[] GetMoves()
    {
        // --- TODO ---
        return new MoveInfo[]
        {
        // Rook 방향 (상하좌우)
        new MoveInfo(1, 0, 0),    // →
        new MoveInfo(-1, 0, 0),   // ←
        new MoveInfo(0, 1, 0),    // ↑
        new MoveInfo(0, -1, 0),   // ↓

        // Bishop 방향 (대각선)
        new MoveInfo(1, 1, 0),    // ↗
        new MoveInfo(-1, 1, 0),   // ↖
        new MoveInfo(-1, -1, 0),  // ↙
        new MoveInfo(1, -1, 0)    // ↘
        };
        // ------
    }
}