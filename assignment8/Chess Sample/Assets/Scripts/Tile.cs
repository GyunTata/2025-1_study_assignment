using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public (int, int) MyPos;    // Tile의 좌표
    Color tileColor = new Color(255 / 255f, 193 / 255f, 204 / 255f);    // 색깔
    SpriteRenderer MySpriteRenderer;

    private void Awake()
    {
        MySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 타일을 처음에 배치하는 함수
    public void Set((int, int) targetPos)
    {
        // MyPos를 targetPos로 지정함
        // 위치를 targetPos 이동시키고, 배치에 따라 색깔을 지정
        // --- TODO ---
        MyPos = targetPos;

        int x = targetPos.Item1;
        int y = targetPos.Item2;

        transform.position = new Vector3(x, 0f, y);

        bool isDark = (x + y) % 2 == 1;
        MySpriteRenderer.color = isDark ? Color.gray : tileColor;
        // ------ 
    }
    // ------
}

