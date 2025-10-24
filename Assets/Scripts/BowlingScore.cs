using UnityEngine;
using TMPro;

public class BowlingScore : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI scoreText;

    [Header("References")]
    [SerializeField] private BowlingPin[] allPins;
    [SerializeField] private Ball ballThrow;

    private int totalScore = 0;
    private int pinsKnockedThisFrame = 0;
    private int currentFrame = 1;
    private int currentThrow = 1;

    void Start()
    {
        UpdateScoreDisplay();
    }

    public void PinKnockedDown()
    {
        pinsKnockedThisFrame++;
        UpdateScoreDisplay();
    }

    public void EndThrow()
    {
        totalScore += pinsKnockedThisFrame;
        RespawnEverything();
    }

    void RespawnEverything()
    {
        foreach (var pin in allPins)
        {
            if (pin != null)
                pin.ResetPin();
        }

        if (ballThrow != null)
        {
            ballThrow.RespawnBall();
        }

        if (currentThrow == 1)
        {
            currentThrow = 2;
        }
        else
        {
            currentThrow = 1;
            currentFrame++;
        }

        pinsKnockedThisFrame = 0;
        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Frame: {currentFrame} | Throw: {currentThrow}\n" +
                           $"Shot down: {pinsKnockedThisFrame}\n" +
                           $"Score: {totalScore}";
        }
    }
}