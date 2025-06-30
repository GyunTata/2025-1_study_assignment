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
        MySpriteRenderer = GetComponent<SpriteRenderer>(); //선언해둔 Spriterenderer할당
    }

    // 타일을 처음에 배치하는 함수
    public void Set((int, int) targetPos)
    {
        // MyPos를 targetPos로 지정함
        // 위치를 targetPos 이동시키고, 배치에 따라 색깔을 지정
        // --- TODO ---
        MyPos = targetPos;

        int x = targetPos.Item1; //목표 좌표의 x 할당
        int y = targetPos.Item2; //목표 좌표의 y 할당

        transform.position = new Vector3(x, y, 0f); //할당된 위치로 transform상 이동

        bool isDark = (x + y) % 2 == 1; //블럭 색깔 따지기
        MySpriteRenderer.color = isDark ? Color.gray : tileColor; //색깔 할당
        // ------ 
    }
    // ------
}

