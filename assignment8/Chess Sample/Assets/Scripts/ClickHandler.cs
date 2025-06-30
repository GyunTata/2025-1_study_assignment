using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    private GameManager gameManager;
    private Piece selectedPiece = null; // 지금 선택된 Piece
    private Vector3 dragOffset;
    private Vector3 originalPosition;
    private bool isDragging = false;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Debug.Log("카메라: " + Camera.main);
    }

    // 마우스의 위치를 (int, int) 좌표로 보정해주는 함수
    private (int, int) GetBoardPosition(Vector3 worldPosition)
    {
        int boardX = Mathf.FloorToInt(worldPosition.x);
        int boardY = Mathf.FloorToInt(worldPosition.z);
        return (boardX, boardY);
    }

    void HandleMouseDown()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = 10f; // 카메라가 z = -10 위치에 있고, 타일은 z = 0에 있으므로 보정
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        var boardPos = GetBoardPosition(mousePosition);
        Debug.Log("👉 마우스 클릭 위치 (월드): " + mousePosition + " / 보드 좌표: " + boardPos);

        if (!Utils.IsInBoard(boardPos))
        {
            Debug.Log("❌ 보드 밖 클릭");
            return;
        }

        Piece clickedPiece = gameManager.Pieces[boardPos.Item1, boardPos.Item2];

        if (clickedPiece == null)
        {
            Debug.Log("❌ 해당 타일에 피스 없음");
            return;
        }

        Debug.Log("✅ 클릭한 피스: " + clickedPiece.name + ", 방향: " + clickedPiece.PlayerDirection);
        Debug.Log("현재 턴: " + gameManager.CurrentTurn);

        if (clickedPiece.PlayerDirection == gameManager.CurrentTurn)
        {
            Debug.Log("🎯 피스 선택 성공!");
            selectedPiece = clickedPiece;
            isDragging = true;
            dragOffset = selectedPiece.transform.position - mousePosition;
            dragOffset.z = 0;
            originalPosition = selectedPiece.transform.position;

            gameManager.ShowPossibleMoves(selectedPiece);
        }
        else
        {
            Debug.Log("❌ 피스는 있지만 내 턴 아님");
        }
    }

    void HandleDrag()
    {
        if (selectedPiece != null) //피스가 있다면
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //mouseposition을 감지
            mousePosition.z = 0;
            selectedPiece.transform.position = mousePosition + dragOffset; //거리유지?
            Debug.Log("드래그 중: " + selectedPiece.name + " → " + selectedPiece.transform.position);
        }
    }

    void HandleMouseUp()
    {
        if (selectedPiece != null) //피스가 있다면
        {
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = 10f;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            var boardPos = GetBoardPosition(mousePosition); // 마우스 위치를 보드칸에 맞게 보정

            // 좌표를 검증함
            // selectedPiece가 움직일 수 있는지를 확인하고, 이동시킴
            // 움직일 수 없다면 selectedPiece를 originalPosition으로 이동시킴
            // effect를 초기화
            // --- TODO ---
            if (Utils.IsInBoard(boardPos) && gameManager.IsValidMove(selectedPiece, boardPos))
            {
                gameManager.Move(selectedPiece, boardPos); // 이동 및 턴 교체
            }
            else
            {
                // 이동 불가 → 제자리로 복귀
                selectedPiece.transform.position = originalPosition;
            }

            gameManager.ClearEffects();
            // ------
            isDragging = false; //드래그 전환 해제
            selectedPiece = null; //선택된 피스 초기화
        }
    }

    void Update()
    {
        // 입력 제어
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseDown();
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            HandleDrag();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            HandleMouseUp();
        }
    }
}