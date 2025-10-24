using UnityEngine;
using TMPro;

public class BowlingScore : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI scoreText;

    private int totalScore = 0;
    private int pinsKnockedThisFrame = 0;
    private int currentFrame = 1;
    private int currentThrow = 1;

    private BowlingPin[] allPins;
    private Ball ballThrow;

    void Start()
    {
        allPins = FindObjectsOfType<BowlingPin>();
        ballThrow = FindObjectOfType<Ball>();
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
            scoreText.text = $"Фрейм: {currentFrame} | Бросок: {currentThrow}\n" +
                           $"Сбито: {pinsKnockedThisFrame}\n" +
                           $"Очки: {totalScore}";
        }
    }
}