using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, ISelectable
{
    [Header("Materials")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private Material moveMaterial;
    [SerializeField] private Material attackMaterial;

    [Header("References")]
    [SerializeField] private Renderer cellRenderer;

    private Vector2Int boardPosition;
    private Unit currentUnit;
    private bool isSelected = false;

    public Vector2Int BoardPosition => boardPosition;
    public Unit CurrentUnit => currentUnit;
    public bool IsSelected => isSelected;

    void Awake()
    {
        if (cellRenderer == null)
            cellRenderer = GetComponent<Renderer>();

        if (cellRenderer != null && defaultMaterial != null)
            cellRenderer.material = defaultMaterial;
    }

    public void Initialize(Vector2Int position, Material cellMaterial = null)
    {
        boardPosition = position;
        gameObject.name = $"Cell_{position.x}_{position.y}";

        if (cellMaterial != null)
            defaultMaterial = cellMaterial;

        if (cellRenderer != null && defaultMaterial != null)
            cellRenderer.material = defaultMaterial;
    }

    public void SetUnit(Unit unit)
    {
        currentUnit = unit;
    }

    public void ClearUnit()
    {
        currentUnit = null;
    }

    public void Select()
    {
        isSelected = true;
        if (cellRenderer != null && selectedMaterial != null)
            cellRenderer.material = selectedMaterial;
    }

    public void Deselect()
    {
        isSelected = false;
        if (cellRenderer != null && defaultMaterial != null)
            cellRenderer.material = defaultMaterial;
    }

    public void Highlight(bool highlight)
    {
        if (!isSelected && cellRenderer != null)
        {
            cellRenderer.material = highlight ? highlightMaterial : defaultMaterial;
        }
    }

    public void SetAsMoveTarget(bool isTarget, bool isAttack = false)
    {
        if (cellRenderer != null)
        {
            if (isTarget)
            {
                cellRenderer.material = isAttack ? attackMaterial : moveMaterial;
            }
            else
            {
                cellRenderer.material = defaultMaterial;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData) => Highlight(true);
    public void OnPointerExit(PointerEventData eventData) => Highlight(false);

    public void OnPointerClick(PointerEventData eventData)
    {
        BattleController.Instance?.OnCellClicked(this);
    }
}
