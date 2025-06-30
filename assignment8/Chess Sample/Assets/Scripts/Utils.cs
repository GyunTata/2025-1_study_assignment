using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    // 타일의 실제 크기(transform상)
    public const int TileSize = 1;  
    // 체스판의 크기(tilesize를 단위로)
    public const int FieldWidth = 8;
    public const int FieldHeight = 8;

    // 체스판상 좌표를 실제 transform 좌표로 쓸 수 있게 변환
    public static Vector2 ToRealPos((int, int) targetPos)
    {
        return TileSize * (new Vector2(
            targetPos.Item1 - (FieldWidth - 1) / 2f, 
            targetPos.Item2 - (FieldHeight - 1) / 2f
        ));
    }

    // 좌표를 받아 체스판 안에 있는지를 따지는 함수
    public static bool IsInBoard((int, int) targetPos)
    {
        (int x, int y) = targetPos;
        return x >= 0 && x < FieldWidth && y >= 0 && y < FieldHeight;
    }
}
