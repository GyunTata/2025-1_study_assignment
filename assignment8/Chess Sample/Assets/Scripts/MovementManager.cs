using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject effectPrefab;
    private Transform effectParent;
    private List<GameObject> currentEffects = new List<GameObject>();   // 현재 effect들을 저장할 리스트
    
    public void Initialize(GameManager gameManager, GameObject effectPrefab, Transform effectParent)
    {
        this.gameManager = gameManager;
        this.effectPrefab = effectPrefab;
        this.effectParent = effectParent;
    }

    private bool TryMove(Piece piece, (int, int) targetPos, MoveInfo moveInfo)
    {
        // moveInfo의 distance만큼 direction을 이동시키며 이동이 가능한지를 체크
        // 보드에 있는지, 다른 piece에 의해 막히는지 등을 체크
        // 폰에 대한 예외 처리를 적용
        // --- TODO ---
        int dx = moveInfo.dirX;
        int dy = moveInfo.dirY;
        int dist = moveInfo.distance;

        int startX = piece.MyPos.Item1;
        int startY = piece.MyPos.Item2;

        for (int i = 1; i <= dist; i++)
        {
            int nextX = startX + dx * i;
            int nextY = startY + dy * i;
            var nextPos = (nextX, nextY);

            if (!Utils.IsInBoard(nextPos))
                return false;

            Piece targetPiece = gameManager.Pieces[nextX, nextY];

            if (piece is Pawn)
            {
                bool isForward = dx == 0;
                bool isDiagonal = dx != 0;

                // 앞으로 가는 경우
                if (isForward)
                {
                    // 도착 위치가 targetPos가 아니면 중간 경로이므로 반드시 비어 있어야 함
                    if (targetPiece != null)
                        return false;

                    if (nextPos == targetPos)
                        return true;
                }

                // 대각선 공격
                else if (isDiagonal && i == 1)
                {
                    if (nextPos == targetPos)
                    {
                        // 대각선에는 상대 기물이 있어야만 가능
                        if (targetPiece != null && targetPiece.PlayerDirection != piece.PlayerDirection)
                            return true;
                        else
                            return false;
                    }
                }

                // 나머지는 안됨 (예: 대각선 2칸 이동 같은 것)
                continue;
            }

            // 일반 기물
            if (nextPos == targetPos)
            {
                if (targetPiece == null)
                    return true;
                else
                    return targetPiece.PlayerDirection != piece.PlayerDirection;
            }

            // 경로에 기물이 있으면 막힘
            if (targetPiece != null)
                return false;
        }

        return false;
        // ------
    }

    // 체크를 제외한 상황에서 가능한 움직임인지를 검증
    private bool IsValidMoveWithoutCheck(Piece piece, (int, int) targetPos)
    {
        if (!Utils.IsInBoard(targetPos) || targetPos == piece.MyPos) return false;

        foreach (var moveInfo in piece.GetMoves())
        {
            if (TryMove(piece, targetPos, moveInfo))
                return true;
        }
        
        return false;
    }

    // 체크를 포함한 상황에서 가능한 움직임인지를 검증
    public bool IsValidMove(Piece piece, (int, int) targetPos)
    {
        if (!IsValidMoveWithoutCheck(piece, targetPos)) return false;

        // 체크 상태 검증을 위한 임시 이동
        var originalPiece = gameManager.Pieces[targetPos.Item1, targetPos.Item2];
        var originalPos = piece.MyPos;

        gameManager.Pieces[targetPos.Item1, targetPos.Item2] = piece;
        gameManager.Pieces[originalPos.Item1, originalPos.Item2] = null;
        piece.MyPos = targetPos;

        bool isValid = !IsInCheck(piece.PlayerDirection);

        // 원상 복구
        gameManager.Pieces[originalPos.Item1, originalPos.Item2] = piece;
        gameManager.Pieces[targetPos.Item1, targetPos.Item2] = originalPiece;
        piece.MyPos = originalPos;

        return isValid;
    }

    // 체크인지를 확인
    private bool IsInCheck(int playerDirection)
    {
        (int, int) kingPos = (-1, -1); // 왕의 위치
        for (int x = 0; x < Utils.FieldWidth; x++)
        {
            for (int y = 0; y < Utils.FieldHeight; y++)
            {
                var piece = gameManager.Pieces[x, y];
                if (piece is King && piece.PlayerDirection == playerDirection)
                {
                    kingPos = (x, y);
                    break;
                }
            }
            if (kingPos.Item1 != -1 && kingPos.Item2 != -1) break;
        }

        // 왕이 지금 체크 상태인지를 리턴
        // gameManager.Pieces에서 Piece들을 참조하여 움직임을 확인
        // --- TODO ---
        for (int x = 0; x < Utils.FieldWidth; x++)
        {
            for (int y = 0; y < Utils.FieldHeight; y++)
            {
                var oppPiece = gameManager.Pieces[x, y];
                if (oppPiece == null || oppPiece.PlayerDirection == playerDirection) continue;

                if (IsValidMoveWithoutCheck(oppPiece, kingPos))
                    return true;     // 하나라도 공격 가능하면 체크
            }
        }
        return false;
        // ------
    }

    public void ShowPossibleMoves(Piece piece)
    {
        ClearEffects();

        // 가능한 움직임을 표시
        // IsValidMove를 사용
        // effectPrefab을 effectParent의 자식으로 생성하고 위치를 적절히 설정
        // currentEffects에 effectPrefab을 추가
        // --- TODO ---
        for (int x = 0; x < Utils.FieldWidth; x++)
        {
            for (int y = 0; y < Utils.FieldHeight; y++)
            {
                var pos = (x, y);
                if (IsValidMove(piece, pos))
                {
                    Vector3 worldPos = new Vector3(x, 0.01f, y);   // 살짝 띄워서 보이게
                    GameObject fx = Instantiate(effectPrefab, worldPos, Quaternion.identity, effectParent);
                    currentEffects.Add(fx);
                }
            }
        }
        // ------
    }

    // 효과 비우기
    public void ClearEffects()
    {
        foreach (var effect in currentEffects)
        {
            if (effect != null) Destroy(effect);
        }
        currentEffects.Clear();
    }
}