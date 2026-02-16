using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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
    private Material currentMaterial;

    public Vector2Int BoardPosition => boardPosition;
    public Unit CurrentUnit => currentUnit;
    public bool IsSelected => isSelected;

    void Awake()
    {
        if (cellRenderer == null)
            cellRenderer = GetComponent<Renderer>();
    }

    public void Initialize(Vector2Int position, Material cellMaterial = null)
    {
        boardPosition = position;
        gameObject.name = $"Cell_{position.x}_{position.y}";

        if (cellMaterial != null)
        {
            defaultMaterial = cellMaterial;
        }

        ResetMaterial();
    }

    void ResetMaterial()
    {
        if (cellRenderer != null && defaultMaterial != null)
        {
            cellRenderer.material = defaultMaterial;
            currentMaterial = defaultMaterial;
        }
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
        {
            cellRenderer.material = selectedMaterial;
            currentMaterial = selectedMaterial;
        }
    }

    public void Deselect()
    {
        isSelected = false;
        ResetMaterial();
    }

    public void Highlight(bool highlight)
    {
        if (!isSelected && cellRenderer != null)
        {
            Material targetMaterial = highlight ? highlightMaterial : defaultMaterial;
            if (targetMaterial != null)
            {
                cellRenderer.material = targetMaterial;
                currentMaterial = targetMaterial;
            }
        }
    }

    public void SetAsMoveTarget(bool isTarget, bool isAttack = false)
    {
        if (cellRenderer != null)
        {
            if (isTarget)
            {
                Material targetMaterial = isAttack ? attackMaterial : moveMaterial;
                if (targetMaterial != null)
                {
                    cellRenderer.material = targetMaterial;
                    currentMaterial = targetMaterial;
                }
            }
            else
            {
                ResetMaterial();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Highlight(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Highlight(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BattleController.Instance?.OnCellClicked(this);
    }
}