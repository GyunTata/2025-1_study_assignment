using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public (int, int) MyPos;    // Piece의 좌표
    public int PlayerDirection = 1; // 자신의 방향 1 - 백, 2 - 흑
    
    public Sprite WhiteSprite;  // 백일 때의 sprite
    public Sprite BlackSprite;  // 흑일 때의 sprite
    
    protected GameManager MyGameManager;
    protected SpriteRenderer MySpriteRenderer;

    void Awake()
    {
        MyGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        MySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Piece의 초기 설정 함수
    public void initialize((int, int) targetPos, int direction) //처음 Piece 배치용 함수, 위치와 방향을 따짐.
    {
        PlayerDirection = direction;
        initSprite(PlayerDirection);
        MoveTo(targetPos);
    }

    // sprite 초기 설정 함수
    void initSprite(int direction) //direction에 따라 Piece의 색과 방향을 결정
    {
        // direction에 따라 sprite를 결정하고, 방향을 결정함
        // --- TODO ---
        if (direction == 1)
        {
            MySpriteRenderer.sprite = WhiteSprite;
            MySpriteRenderer.flipY = false;
        }
        else
        {
            MySpriteRenderer.sprite = BlackSprite;
            MySpriteRenderer.flipY = true;
        }
        // ------
    }

    // piece의 실제 이동 함수
    public void MoveTo((int, int) targetPos)
    {
        // MyPos를 업데이트하고, targetPos로 이동
        // MyGameManager.Pieces를 업데이트
        // --- TODO ---
        MyGameManager.Pieces[MyPos.Item1, MyPos.Item2] = null; //리스트 상 Piece 위치 제거

        // 새 위치 갱신
        MyPos = targetPos; //변수적 위치 적용
        transform.position = new Vector3(targetPos.Item1, targetPos.Item2, 0f); //transform상 위치 적용

        // 새 위치에 자신 등록
        MyGameManager.Pieces[MyPos.Item1, MyPos.Item2] = this; //리스트 상 Piece 위치 추가
        // ------
    }
    
    public abstract MoveInfo[] GetMoves();
}
