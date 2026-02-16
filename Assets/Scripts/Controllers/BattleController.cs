using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleController : MonoBehaviour
{
    public static BattleController Instance { get; private set; }

    [Header("Board Settings")]
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Material whiteCellMaterial;
    [SerializeField] private Material blackCellMaterial;

    [Header("Piece Prefabs - White")]
    [SerializeField] private GameObject whitePawnPrefab;
    [SerializeField] private GameObject whiteKnightPrefab;
    [SerializeField] private GameObject whiteBishopPrefab;
    [SerializeField] private GameObject whiteRookPrefab;
    [SerializeField] private GameObject whiteQueenPrefab;
    [SerializeField] private GameObject whiteKingPrefab;

    [Header("Piece Prefabs - Black")]
    [SerializeField] private GameObject blackPawnPrefab;
    [SerializeField] private GameObject blackKnightPrefab;
    [SerializeField] private GameObject blackBishopPrefab;
    [SerializeField] private GameObject blackRookPrefab;
    [SerializeField] private GameObject blackQueenPrefab;
    [SerializeField] private GameObject blackKingPrefab;

    [Header("UI")]
    [SerializeField] private PromotionUI promotionUI;
    [SerializeField] private Text turnText;

    [Header("References")]
    [SerializeField] private PlayerController playerController;

    private Cell[,] board = new Cell[8, 8];
    private List<Unit> allUnits = new List<Unit>();
    private Unit selectedUnit;
    private List<Cell> highlightedCells = new List<Cell>();
    private GameState currentState = GameState.WhiteTurn;
    private Unit pawnToPromote;

    private Unit whiteKing;
    private Unit blackKing;
    private bool isCheck;
    private Pawn enPassantPawn;
    private int enPassantTurnCounter;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CreateBoard();
        SetupPieces();
        FindKings();
        UpdateTurnText();
    }

    void FindKings()
    {
        foreach (Unit unit in allUnits)
        {
            if (unit.PieceType == PieceType.King)
            {
                if (unit.Team == Team.White)
                    whiteKing = unit;
                else
                    blackKing = unit;
            }
        }
    }

    void CreateBoard()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                GameObject cellObj = Instantiate(cellPrefab, transform);
                cellObj.transform.position = new Vector3(x, 0.01f, y);
                cellObj.name = $"Cell_{x}_{y}";

                Cell cell = cellObj.GetComponent<Cell>();
                Material cellMaterial = (x + y) % 2 == 0 ? whiteCellMaterial : blackCellMaterial;
                cell.Initialize(new Vector2Int(x, y), cellMaterial);

                board[x, y] = cell;
            }
        }
    }

    void SetupPieces()
    {
        CreatePiece(whiteRookPrefab, Team.White, 0, 0);
        CreatePiece(whiteKnightPrefab, Team.White, 1, 0);
        CreatePiece(whiteBishopPrefab, Team.White, 2, 0);
        CreatePiece(whiteQueenPrefab, Team.White, 3, 0);
        CreatePiece(whiteKingPrefab, Team.White, 4, 0);
        CreatePiece(whiteBishopPrefab, Team.White, 5, 0);
        CreatePiece(whiteKnightPrefab, Team.White, 6, 0);
        CreatePiece(whiteRookPrefab, Team.White, 7, 0);

        for (int x = 0; x < 8; x++)
            CreatePiece(whitePawnPrefab, Team.White, x, 1);

        CreatePiece(blackRookPrefab, Team.Black, 0, 7);
        CreatePiece(blackKnightPrefab, Team.Black, 1, 7);
        CreatePiece(blackBishopPrefab, Team.Black, 2, 7);
        CreatePiece(blackQueenPrefab, Team.Black, 3, 7);
        CreatePiece(blackKingPrefab, Team.Black, 4, 7);
        CreatePiece(blackBishopPrefab, Team.Black, 5, 7);
        CreatePiece(blackKnightPrefab, Team.Black, 6, 7);
        CreatePiece(blackRookPrefab, Team.Black, 7, 7);

        for (int x = 0; x < 8; x++)
            CreatePiece(blackPawnPrefab, Team.Black, x, 6);
    }

    void CreatePiece(GameObject prefab, Team team, int x, int y)
    {
        if (prefab == null) return;

        Cell cell = GetCell(x, y);
        if (cell == null) return;

        GameObject pieceObj = Instantiate(prefab, transform);
        Unit unit = pieceObj.GetComponent<Unit>();
        unit.Initialize(cell, team);
        allUnits.Add(unit);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleClick();

        if (Input.GetKeyDown(KeyCode.Escape))
            CancelSelection();
    }

    void HandleClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Cell cell = hit.collider.GetComponent<Cell>();
            if (cell != null)
                OnCellClicked(cell);
        }
    }

    public void OnCellClicked(Cell cell)
    {
        if (playerController != null && playerController.IsInputBlocked)
            return;

        if (pawnToPromote != null)
            return;

        if (selectedUnit != null)
        {
            if (highlightedCells.Contains(cell))
            {
                ExecuteMove(selectedUnit, cell);
                return;
            }
            else
            {
                CancelSelection();
            }
        }

        if (cell.CurrentUnit != null)
        {
            Unit unit = cell.CurrentUnit;

            bool canSelect = (currentState == GameState.WhiteTurn && unit.Team == Team.White) ||
                            (currentState == GameState.BlackTurn && unit.Team == Team.Black);

            if (canSelect)
                SelectUnit(unit);
        }
    }
    void SelectUnit(Unit unit)
    {
        CancelSelection();

        selectedUnit = unit;
        selectedUnit.Select();

        List<Vector2Int> allMoves = unit.GetPossibleMoves();
        List<Vector2Int> safeMoves = FilterSafeMoves(unit, allMoves);

        HighlightMoves(safeMoves);
    }

    List<Vector2Int> FilterSafeMoves(Unit unit, List<Vector2Int> moves)
    {
        if (unit.PieceType != PieceType.King)
            return moves;

        List<Vector2Int> safeMoves = new List<Vector2Int>();

        foreach (Vector2Int move in moves)
        {
            bool isAttacked = false;

            foreach (Unit enemy in allUnits)
            {
                if (enemy.Team == unit.Team)
                    continue;

                if (enemy.PieceType == PieceType.Pawn)
                {
                    if (IsPawnAttacking(enemy, move))
                    {
                        isAttacked = true;
                        break;
                    }
                }
                else
                {
                    List<Vector2Int> enemyMoves = enemy.GetPossibleMoves();
                    if (enemyMoves.Contains(move))
                    {
                        isAttacked = true;
                        break;
                    }
                }
            }

            if (!isAttacked)
                safeMoves.Add(move);
        }

        return safeMoves;
    }

    bool IsPawnAttacking(Unit pawn, Vector2Int targetPos)
    {
        Vector2Int pawnPos = pawn.BoardPosition;
        int direction = (pawn.Team == Team.White) ? 1 : -1;

        Vector2Int[] attacks = {
            new Vector2Int(pawnPos.x + 1, pawnPos.y + direction),
            new Vector2Int(pawnPos.x - 1, pawnPos.y + direction)
        };

        foreach (Vector2Int attack in attacks)
        {
            if (attack == targetPos)
                return true;
        }

        return false;
    }

    public bool IsKingInCheck(Team team)
    {
        Unit king = (team == Team.White) ? whiteKing : blackKing;
        if (king == null) return false;

        Vector2Int kingPos = king.BoardPosition;

        foreach (Unit unit in allUnits)
        {
            if (unit.Team != team)
            {
                List<Vector2Int> moves = unit.GetPossibleMoves();
                foreach (Vector2Int move in moves)
                {
                    if (move == kingPos)
                        return true;
                }
            }
        }

        return false;
    }

    void UpdateCheckStatus()
    {
        bool whiteInCheck = IsKingInCheck(Team.White);
        bool blackInCheck = IsKingInCheck(Team.Black);
        isCheck = whiteInCheck || blackInCheck;
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

    void ClearAllCells()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (board[x, y] != null)
                {
                    board[x, y].SetAsMoveTarget(false);
                    board[x, y].Deselect();
                }
            }
        }
        highlightedCells.Clear();
    }

    void ExecuteMove(Unit unit, Cell targetCell)
    {
        bool isCastling = (unit.PieceType == PieceType.King &&
                          Mathf.Abs(targetCell.BoardPosition.x - unit.BoardPosition.x) == 2);

        if (!isCastling && targetCell.CurrentUnit != null && targetCell.CurrentUnit.Team != unit.Team)
        {
            targetCell.CurrentUnit.Capture();
            allUnits.Remove(targetCell.CurrentUnit);
        }

        if (isCastling)
            PerformCastling(unit, targetCell.BoardPosition);

        unit.MoveTo(targetCell);
        UpdateCheckStatus();

        if (unit.PieceType == PieceType.Pawn)
        {
            int promotionRow = (unit.Team == Team.White) ? 7 : 0;
            if (targetCell.BoardPosition.y == promotionRow)
            {
                RequestPawnPromotion(unit);
                return;
            }
        }

        CompleteMove();
    }

    public void RequestPawnPromotion(Unit pawn)
    {
        pawnToPromote = pawn;
        ShowPromotionUI();
    }

    void ShowPromotionUI()
    {
        if (promotionUI != null)
        {
            promotionUI.Show(pawnToPromote);
            if (playerController != null)
                playerController.BlockInput(true);
        }
    }

    public void PromotePawn(PieceType newType)
    {
        if (pawnToPromote == null) return;

        Team team = pawnToPromote.Team;
        Vector2Int position = pawnToPromote.BoardPosition;

        allUnits.Remove(pawnToPromote);
        pawnToPromote.Capture();

        GameObject prefab = GetPrefab(team, newType);
        if (prefab != null)
        {
            Cell cell = GetCell(position.x, position.y);
            GameObject newPieceObj = Instantiate(prefab, transform);
            Unit newUnit = newPieceObj.GetComponent<Unit>();
            newUnit.Initialize(cell, team);
            allUnits.Add(newUnit);
        }

        pawnToPromote = null;

        if (promotionUI != null)
            promotionUI.Hide();

        if (playerController != null)
            playerController.BlockInput(false);

        CompleteMove();
    }

    GameObject GetPrefab(Team team, PieceType type)
    {
        if (team == Team.White)
        {
            return type switch
            {
                PieceType.Pawn => whitePawnPrefab,
                PieceType.Knight => whiteKnightPrefab,
                PieceType.Bishop => whiteBishopPrefab,
                PieceType.Rook => whiteRookPrefab,
                PieceType.Queen => whiteQueenPrefab,
                PieceType.King => whiteKingPrefab,
                _ => null
            };
        }
        else
        {
            return type switch
            {
                PieceType.Pawn => blackPawnPrefab,
                PieceType.Knight => blackKnightPrefab,
                PieceType.Bishop => blackBishopPrefab,
                PieceType.Rook => blackRookPrefab,
                PieceType.Queen => blackQueenPrefab,
                PieceType.King => blackKingPrefab,
                _ => null
            };
        }
    }

    void CompleteMove()
    {
        ClearAllCells();

        if (selectedUnit != null)
        {
            selectedUnit.Deselect();
            selectedUnit = null;
        }

        SwitchTurn();
    }

    void CancelSelection()
    {
        ClearAllCells();

        if (selectedUnit != null)
        {
            selectedUnit.Deselect();
            selectedUnit = null;
        }
    }

    void SwitchTurn()
    {
        currentState = currentState == GameState.WhiteTurn ?
            GameState.BlackTurn : GameState.WhiteTurn;

        UpdateEnPassant();

        UpdateCheckStatus();
        UpdateTurnText();
    }

    void UpdateTurnText()
    {
        if (turnText != null)
        {
            if (isCheck)
                turnText.text = "״ְױ!";
            else
                turnText.text = currentState == GameState.WhiteTurn ? "ױמה בוכûץ" : "ױמה קונםûץ";
        }
    }

    public Cell GetCell(int x, int y)
    {
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
            return board[x, y];
        return null;
    }

    public List<Unit> GetAllUnits()
    {
        return allUnits;
    }

    public void PerformCastling(Unit king, Vector2Int targetPos)
    {
        int row = king.BoardPosition.y;
        int rookFromX, rookToX;

        if (targetPos.x == 6)
        {
            rookFromX = 7;
            rookToX = 5;
        }
        else if (targetPos.x == 2)
        {
            rookFromX = 0;
            rookToX = 3;
        }
        else
        {
            return;
        }

        Cell rookCell = GetCell(rookFromX, row);
        if (rookCell.CurrentUnit != null && rookCell.CurrentUnit.PieceType == PieceType.Rook)
        {
            Unit rook = rookCell.CurrentUnit;
            Cell targetRookCell = GetCell(rookToX, row);
            rook.MoveTo(targetRookCell);
        }
    }
    public void SetEnPassantTarget(Pawn pawn, Vector2Int targetPos)
    {
        enPassantPawn = pawn;
        enPassantTurnCounter = 0;
    }
    void UpdateEnPassant()
    {
        if (enPassantPawn != null)
        {
            enPassantTurnCounter++;
            if (enPassantTurnCounter >= 2)
            {
                enPassantPawn.ResetJustMovedTwoForward();
                enPassantPawn = null;
            }
        }
    }
    public void RemoveUnit(Unit unit)
    {
        allUnits.Remove(unit);
    }
}