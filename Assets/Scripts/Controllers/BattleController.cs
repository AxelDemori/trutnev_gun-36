using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController Instance { get; private set; }

    [Header("Board Settings")]
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Material whiteCellMaterial;
    [SerializeField] private Material blackCellMaterial;

    [Header("Piece Prefabs")]
    [SerializeField] private GameObject whitePawnPrefab;
    [SerializeField] private GameObject whiteKnightPrefab;
    [SerializeField] private GameObject whiteBishopPrefab;
    [SerializeField] private GameObject whiteRookPrefab;
    [SerializeField] private GameObject whiteQueenPrefab;
    [SerializeField] private GameObject whiteKingPrefab;

    [SerializeField] private GameObject blackPawnPrefab;
    [SerializeField] private GameObject blackKnightPrefab;
    [SerializeField] private GameObject blackBishopPrefab;
    [SerializeField] private GameObject blackRookPrefab;
    [SerializeField] private GameObject blackQueenPrefab;
    [SerializeField] private GameObject blackKingPrefab;

    [Header("References")]
    [SerializeField] private PlayerController playerController;

    
    private Cell[,] board = new Cell[8, 8];
    private List<Unit> allUnits = new List<Unit>();

    
    private Unit selectedUnit;
    private List<Cell> highlightedCells = new List<Cell>();
    private GameState currentState = GameState.WhiteTurn;
    private Unit pawnToPromote;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        CreateBoard();
        SetupPieces();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelSelection();
        }
    }

    void CreateBoard()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                GameObject cellObj = Instantiate(cellPrefab, transform);
                cellObj.transform.position = new Vector3(x, 0, y);

                Cell cell = cellObj.GetComponent<Cell>();
                Material cellMaterial = (x + y) % 2 == 0 ? whiteCellMaterial : blackCellMaterial;
                cell.Initialize(new Vector2Int(x, y), cellMaterial);

                board[x, y] = cell;
            }
        }
    }

    void SetupPieces()
    {
        
        CreatePiece(whiteRookPrefab, 0, 0);
        CreatePiece(whiteKnightPrefab, 1, 0);
        CreatePiece(whiteBishopPrefab, 2, 0);
        CreatePiece(whiteQueenPrefab, 3, 0);
        CreatePiece(whiteKingPrefab, 4, 0);
        CreatePiece(whiteBishopPrefab, 5, 0);
        CreatePiece(whiteKnightPrefab, 6, 0);
        CreatePiece(whiteRookPrefab, 7, 0);

        for (int x = 0; x < 8; x++)
        {
            CreatePiece(whitePawnPrefab, x, 1);
        }

        
        CreatePiece(blackRookPrefab, 0, 7);
        CreatePiece(blackKnightPrefab, 1, 7);
        CreatePiece(blackBishopPrefab, 2, 7);
        CreatePiece(blackQueenPrefab, 3, 7);
        CreatePiece(blackKingPrefab, 4, 7);
        CreatePiece(blackBishopPrefab, 5, 7);
        CreatePiece(blackKnightPrefab, 6, 7);
        CreatePiece(blackRookPrefab, 7, 7);

        for (int x = 0; x < 8; x++)
        {
            CreatePiece(blackPawnPrefab, x, 6);
        }
    }

    void CreatePiece(GameObject prefab, int x, int y)
    {
        Cell cell = GetCell(x, y);
        if (cell == null || prefab == null)
            return;

        GameObject pieceObj = Instantiate(prefab, transform);
        Unit unit = pieceObj.GetComponent<Unit>();

        if (unit == null)
        {
            Debug.LogError("Prefab doesn't have Unit component!");
            Destroy(pieceObj);
            return;
        }

        unit.Initialize(cell);
        allUnits.Add(unit);
    }

    public void OnUnitClicked(Unit unit)
    {
        if (playerController.IsInputBlocked || pawnToPromote != null)
            return;


        bool canSelect = (currentState == GameState.WhiteTurn && unit.Team == Team.White) ||
                         (currentState == GameState.BlackTurn && unit.Team == Team.Black);

        if (canSelect)
        {
            SelectUnit(unit);
        }
        else if (selectedUnit != null)
        {

            Cell targetCell = unit.CurrentCell;
            if (highlightedCells.Contains(targetCell))
            {
                ExecuteMove(selectedUnit, targetCell);
            }
        }
    }

    public void OnCellClicked(Cell cell)
    {
        if (playerController.IsInputBlocked || pawnToPromote != null)
            return;

        if (selectedUnit != null && highlightedCells.Contains(cell))
        {
            ExecuteMove(selectedUnit, cell);
        }
        else
        {
            CancelSelection();
        }
    }

    void SelectUnit(Unit unit)
    {
        CancelSelection();

        selectedUnit = unit;
        selectedUnit.Select();


        List<Vector2Int> moves = unit.GetPossibleMoves();
        HighlightMoves(moves);
    }

    void HighlightMoves(List<Vector2Int> moves)
    {
        highlightedCells.Clear();

        foreach (Vector2Int move in moves)
        {
            Cell cell = GetCell(move.x, move.y);
            if (cell != null)
            {
                bool isAttack = cell.CurrentUnit != null && cell.CurrentUnit.Team != selectedUnit.Team;
                cell.SetAsMoveTarget(true, isAttack);
                highlightedCells.Add(cell);
            }
        }
    }

    void ClearHighlights()
    {
        foreach (Cell cell in highlightedCells)
        {
            cell.SetAsMoveTarget(false);
        }
        highlightedCells.Clear();
    }

    void ExecuteMove(Unit unit, Cell targetCell)
    {

        MoveCommand moveCommand = new MoveCommand(unit, targetCell);


        playerController.ExecuteCommand(moveCommand, OnMoveComplete);
    }

    void OnMoveComplete()
    {
        CancelSelection();
        SwitchTurn();
    }

    void SwitchTurn()
    {
        currentState = currentState == GameState.WhiteTurn
            ? GameState.BlackTurn
            : GameState.WhiteTurn;
    }

    public void CancelSelection()
    {
        if (selectedUnit != null)
        {
            selectedUnit.Deselect();
            selectedUnit = null;
        }

        ClearHighlights();
    }


    public void RequestPawnPromotion(Unit pawn)
    {
        pawnToPromote = pawn;

        Debug.Log($"{pawn.Team} пешка достигла конца доски!");


        pawn.Promote(PieceType.Queen);
        pawnToPromote = null;
    }


    public Cell GetCell(int x, int y)
    {
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
            return board[x, y];
        return null;
    }

    public Cell GetCell(Vector2Int position)
    {
        return GetCell(position.x, position.y);
    }

}