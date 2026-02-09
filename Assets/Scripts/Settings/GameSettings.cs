using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Chess/Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("Board Settings")]
    public Material whiteCellMaterial;
    public Material blackCellMaterial;
    public Material highlightMaterial;
    public Material selectedMaterial;
    public Material moveMaterial;
    public Material attackMaterial;

    [Header("Game Rules")]
    public bool enableCastling = true;
    public bool enableEnPassant = true;
    public bool enablePromotion = true;
}
