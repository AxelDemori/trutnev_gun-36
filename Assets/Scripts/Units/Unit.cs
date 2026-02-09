using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Unit : MonoBehaviour, ISelectable
{
    [Header("Unit Data")]
    [SerializeField] protected PieceType pieceType;
    [SerializeField] protected Team team;

    [Header("Visual")]
    [SerializeField] protected GameObject selectionIndicator;

    protected Cell currentCell;
    protected bool isFirstMove = true;

    // Свойства
    public PieceType PieceType => pieceType;
    public Team Team => team;
    public Cell CurrentCell => currentCell;
    public bool IsFirstMove => isFirstMove;
    public Vector2Int BoardPosition => currentCell?.BoardPosition ?? new Vector2Int(-1, -1);

    // Направления для фигур (можно переопределить в наследниках)
    protected Vector2Int[] rookDirections = {
        new Vector2Int(0, 1), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(-1, 0)
    };

    protected Vector2Int[] bishopDirections = {
        new Vector2Int(1, 1), new Vector2Int(1, -1),
        new Vector2Int(-1, -1), new Vector2Int(-1, 1)
    };

    void Awake()
    {
        if (selectionIndicator != null)
            selectionIndicator.SetActive(false);
    }

    public virtual void Initialize(Cell startCell)
    {
        currentCell = startCell;
        currentCell.SetUnit(this);

        // Позиционируем фигуру над клеткой
        transform.position = startCell.transform.position + Vector3.up * 0.5f;

        Deselect();
    }

    public virtual void MoveTo(Cell targetCell)
    {
        if (currentCell != null)
            currentCell.ClearUnit();

        currentCell = targetCell;
        targetCell.SetUnit(this);

        transform.position = targetCell.transform.position + Vector3.up * 0.5f;

        if (isFirstMove)
            isFirstMove = false;
    }

    // Абстрактный метод для получения возможных ходов
    public abstract List<Vector2Int> GetPossibleMoves();

    // Вспомогательные методы для вычисления ходов
    protected List<Vector2Int> GetSlidingMoves(Vector2Int[] directions, int maxSteps = 7)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        Vector2Int startPos = BoardPosition;

        foreach (Vector2Int dir in directions)
        {
            for (int step = 1; step <= maxSteps; step++)
            {
                Vector2Int nextPos = new Vector2Int(
                    startPos.x + dir.x * step,
                    startPos.y + dir.y * step
                );

                if (!IsWithinBoard(nextPos))
                    break;

                Cell targetCell = BattleController.Instance?.GetCell(nextPos.x, nextPos.y);
                if (targetCell == null)
                    continue;

                // Если клетка пустая - можно ходить
                if (targetCell.CurrentUnit == null)
                {
                    moves.Add(nextPos);
                }
                // Если клетка занята врагом - можно атаковать, но дальше нельзя
                else if (targetCell.CurrentUnit.Team != team)
                {
                    moves.Add(nextPos);
                    break;
                }
                // Если клетка занята союзником - нельзя ходить
                else
                {
                    break;
                }
            }
        }

        return moves;
    }

    protected bool IsWithinBoard(Vector2Int gridPoint)
    {
        return gridPoint.x >= 0 && gridPoint.x < 8 &&
               gridPoint.y >= 0 && gridPoint.y < 8;
    }

    // Реализация ISelectable
    public virtual void Select()
    {
        if (selectionIndicator != null)
            selectionIndicator.SetActive(true);

        currentCell?.Select();
    }

    public virtual void Deselect()
    {
        if (selectionIndicator != null)
            selectionIndicator.SetActive(false);

        currentCell?.Deselect();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        currentCell?.Highlight(true);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        currentCell?.Highlight(false);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        BattleController.Instance?.OnUnitClicked(this);
    }

    public virtual void Capture()
    {
        currentCell?.ClearUnit();
        Destroy(gameObject);
    }

    // Для превращения пешки
    public virtual void Promote(PieceType newType)
    {
        pieceType = newType;
        // Здесь можно поменять модель фигуры
    }
}
