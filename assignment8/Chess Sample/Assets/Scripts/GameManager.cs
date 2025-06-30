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
        for (int x = 0; x < Utils.FieldWidth; x++)
        {
            for (int y = 0; y < Utils.FieldHeight; y++)
            {
                GameObject tileObj = Instantiate(TilePrefab, TileParent);
                Tile tile = tileObj.GetComponent<Tile>();

                (int, int) pos = (x, y);
                tile.Set(pos); // 위치와 색 설정

                Tiles[x, y] = tile;
            }
        }

        // ------

        PlacePieces(1); //타일 깐 이후 기물깔기
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
        GameObject pieceObj = Instantiate(PiecePrefabs[pieceType], PieceParent);
        Piece piece = pieceObj.GetComponent<Piece>();

        piece.initialize(pos, direction); // 위치 및 방향 설정
        Pieces[pos.Item1, pos.Item2] = piece;

        return piece;
        // ------
    }

    public bool IsValidMove(Piece piece, (int, int) targetPos) //movementmanager를 이용해 이동가능한지 판단
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
        (int x0, int y0) = piece.MyPos; //기물의 좌표 가져옴

        // 움직일 위치에 기물이 있다면 제거
        Piece targetPiece = Pieces[targetPos.Item1, targetPos.Item2];
        if (targetPiece != null)
        {
            Destroy(targetPiece.gameObject);
        }

        // 원래 있던 위치 비우기
        Pieces[x0, y0] = null;
        Pieces[targetPos.Item1, targetPos.Item2] = piece; //새 위치에 기물 작성

        // 말 실제 이동
        piece.transform.position = new Vector3(targetPos.Item1, targetPos.Item2, 0f);
        piece.MyPos = targetPos;

        // 효과 정리 및 턴 전환
        ClearEffects();  // 남아있는 이동 표시 제거
        ChangeTurn();    // 턴 교체 및 UI 반영
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
