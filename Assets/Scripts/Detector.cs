using UnityEngine;

public class SimpleThrowDetector : MonoBehaviour
{
    public float respawnDelay = 3f;

    private BowlingScore scoreManager;
    private bool isThrowInProgress = false;

    void Start()
    {
        scoreManager = GetComponent<BowlingScore>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isThrowInProgress)
        {
            isThrowInProgress = true;

            Invoke("RespawnEverything", respawnDelay);
        }
    }

    void RespawnEverything()
    {
        isThrowInProgress = false;
        scoreManager?.EndThrow();
        
    }
}