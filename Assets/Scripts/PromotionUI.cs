using UnityEngine;
using UnityEngine.UI;

public class PromotionUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Button queenButton;
    [SerializeField] private Button rookButton;
    [SerializeField] private Button bishopButton;
    [SerializeField] private Button knightButton;
    [SerializeField] private Text titleText;

    private Unit pawnToPromote;

    void Start()
    {
        if (panel != null)
            panel.SetActive(false);

        queenButton.onClick.AddListener(() => Promote(PieceType.Queen));
        rookButton.onClick.AddListener(() => Promote(PieceType.Rook));
        bishopButton.onClick.AddListener(() => Promote(PieceType.Bishop));
        knightButton.onClick.AddListener(() => Promote(PieceType.Knight));
    }

    public void Show(Unit pawn)
    {
        pawnToPromote = pawn;
        panel.SetActive(true);

        if (titleText != null)
        {
            string teamText = pawn.Team == Team.White ? "аекни" : "вепмни";
            titleText.text = $"опебпюыемхе {teamText} оеьйх";
        }
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    private void Promote(PieceType newType)
    {
        if (pawnToPromote != null)
            BattleController.Instance.PromotePawn(newType);
    }
}