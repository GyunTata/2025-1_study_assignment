using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 프리팹들
    public GameObject TilePrefab;
    public GameObject[] PiecePrefabs;   // King, Queen, Bishop, Knight, Rook, Pawn 순
    public GameObject EffectPrefab;

    // 오브젝트의 parent들
    private Transform TileParent;
    private Transform PieceParent;
    private Transform EffectParent;
    
    private MovementManager movementManager;
    private UIManager uiManager;
    
    public int CurrentTurn = 1; // 현재 턴 1 - 백, 2 - 흑
    public Tile[,] Tiles = new Tile[Utils.FieldWidth, Utils.FieldHeight];   // Tile들
    public Piece[,] Pieces = new Piece[Utils.FieldWidth, Utils.FieldHeight];    // Piece들

    void Awake()
    {
        TileParent = GameObject.Find("TileParent").transform;
        PieceParent = GameObject.Find("PieceParent").transform;
        EffectParent = GameObject.Find("EffectParent").transform;
        
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        movementManager = gameObject.AddComponent<MovementManager>();
        movementManager.Initialize(this, EffectPrefab, EffectParent);
        
        InitializeBoard();
    }

    void InitializeBoard()
    {
        // 8x8로 타일들을 배치
        // TilePrefab을 TileParent의 자식으로 생성하고, 배치함
        // Tiles를 채움
        // --- TODO ---
        for (int y = 0; y < Utils.FieldHeight; y++)
        {
            for (int x = 0; x < Utils.FieldWidth; x++)
            {
                Vector3 worldPos = new Vector3(x, 0f, y);          // 간단히 (x,0,z) 격자
                GameObject tileObj = Instantiate(TilePrefab, worldPos, Quaternion.identity, TileParent);
                tileObj.name = $"Tile_{x}_{y}";

                Tile tile = tileObj.GetComponent<Tile>();
                // 타일 스크립트가 좌표 초기화 함수를 갖고 있다면 호출
                if (tile != null)
                    tile.Set((x, y));

                Tiles[x, y] = tile;
            }
        }
        // ------

        PlacePieces(1);
        PlacePieces(-1);
    }

    void PlacePieces(int direction)
    {
        // PlacePiece를 사용하여 Piece들을 적절한 모양으로 배치
        // --- TODO ---
        int backRank = direction == 1 ? 0 : Utils.FieldHeight - 1;     // 왕·퀸·기타
        int pawnRank = direction == 1 ? 1 : Utils.FieldHeight - 2;     // 폰

        // Major pieces (Rook, Knight, Bishop, Queen, King, Bishop, Knight, Rook)
        PlacePiece(4, (0, backRank), direction); // R
        PlacePiece(3, (1, backRank), direction); // N
        PlacePiece(2, (2, backRank), direction); // B
        PlacePiece(1, (3, backRank), direction); // Q
        PlacePiece(0, (4, backRank), direction); // K
        PlacePiece(2, (5, backRank), direction); // B
        PlacePiece(3, (6, backRank), direction); // N
        PlacePiece(4, (7, backRank), direction); // R

        // Pawns
        for (int x = 0; x < Utils.FieldWidth; x++)
            PlacePiece(5, (x, pawnRank), direction);
        // ------
    }

    Piece PlacePiece(int pieceType, (int, int) pos, int direction)
    {
        // Piece를 배치 후, initialize
        // PiecePrefabs의 원소를 사용하여 배치, PieceParent의 자식으로 생성
        // Pieces를 채움
        // 배치한 Piece를 리턴
        // --- TODO ---
        if (pieceType < 0 || pieceType >= PiecePrefabs.Length)
        {
            Debug.LogError($"PieceType {pieceType}가 유효하지 않음");
            return null;
        }

        GameObject prefab = PiecePrefabs[pieceType];
        Vector3 worldPos = new Vector3(pos.Item1, 0f, pos.Item2);
        GameObject obj = Instantiate(prefab, worldPos, Quaternion.identity, PieceParent);
        obj.name = $"{prefab.name}_{pos.Item1}_{pos.Item2}";

        Piece piece = obj.GetComponent<Piece>();
        if (piece != null)
        {
            piece.MyPos = pos;
            piece.PlayerDirection = direction;

            // gameManager 참조 직접 대입 (protected일 경우 접근 보장)
            var field = typeof(Piece).GetField("gameManager", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
                field.SetValue(piece, this);
        }

        Pieces[pos.Item1, pos.Item2] = piece;
        return piece;
        // ------
    }

    public bool IsValidMove(Piece piece, (int, int) targetPos)
    {
        return movementManager.IsValidMove(piece, targetPos);
    }

    public void ShowPossibleMoves(Piece piece)
    {
        movementManager.ShowPossibleMoves(piece);
    }

    public void ClearEffects()
    {
        movementManager.ClearEffects();
    }


    public void Move(Piece piece, (int, int) targetPos)
    {
        if (!IsValidMove(piece, targetPos)) return;

        // 해당 위치에 다른 Piece가 있다면 삭제
        // Piece를 이동시킴
        // --- TODO ---
        (int x0, int y0) = piece.MyPos;

        // 상대 말을 제거 (캡처)
        Piece targetPiece = Pieces[targetPos.Item1, targetPos.Item2];
        if (targetPiece != null)
        {
            Destroy(targetPiece.gameObject);
        }

        // 보드 상태 갱신
        Pieces[x0, y0] = null;
        Pieces[targetPos.Item1, targetPos.Item2] = piece;

        // 실제 이동
        piece.transform.position = new Vector3(targetPos.Item1, 0f, targetPos.Item2);
        piece.MyPos = targetPos;

        ChangeTurn();  // 턴 넘기기
        // ------
    }

    void ChangeTurn()
    {
        // 턴을 변경하고, UI에 표시
        // --- TODO ---
        CurrentTurn = CurrentTurn == 1 ? 2 : 1;
        uiManager?.UpdateTurn(CurrentTurn);
        // ------
    }
}
