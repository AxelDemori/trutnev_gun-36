using UnityEngine;

public class BowlingPin : MonoBehaviour
{
    private bool isKnockedDown = false;
    private BowlingScore scoreManager;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        scoreManager = FindObjectOfType<BowlingScore>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (!isKnockedDown && Vector3.Angle(transform.up, Vector3.up) > 45f)
        {
            isKnockedDown = true;
            scoreManager?.PinKnockedDown();
        }
    }

    public void ResetPin()
    {
        isKnockedDown = false;
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public bool IsKnockedDown()
    {
        return isKnockedDown;
    }
}
