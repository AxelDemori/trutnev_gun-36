using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Настройки спавна")]
    public Vector3 spawnPosition = new Vector3(0, 0.5f, 0);

    [Header("Настройки броска")]
    public float throwForce = 20f;
    public float upwardForce = 5f;
    public Vector3 throwDirection = Vector3.forward;

    [Header("Префабы")]
    public GameObject ballPrefab;

    private GameObject currentBall;
    private Rigidbody ballRigidbody;
    private bool isBallReady = false;

    void Start()
    {
        RespawnBall();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isBallReady && currentBall != null)
        {
            ThrowBall();
        }
    }

    public void RespawnBall()
    {
        if (currentBall != null)
        {
            Destroy(currentBall);
        }

        currentBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        ballRigidbody = currentBall.GetComponent<Rigidbody>();

        if (ballRigidbody != null)
        {
            ballRigidbody.isKinematic = true;
            isBallReady = true;
        }
    
    }

    void ThrowBall()
    {
        if (ballRigidbody != null)
        {
            ballRigidbody.isKinematic = false;
            Vector3 force = throwDirection.normalized * throwForce + Vector3.up * upwardForce;
            ballRigidbody.AddForce(force, ForceMode.Impulse);

        }

        isBallReady = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spawnPosition, 0.2f);

        Gizmos.color = Color.red;
        Vector3 endPoint = spawnPosition + throwDirection.normalized * 2f;
        Gizmos.DrawLine(spawnPosition, endPoint);
    }
}
