using UnityEngine;
using System.Collections;

public class Cleaner : MonoBehaviour
{
    [Header("Motion Settings")]
    public float moveSpeed = 2f;          
    public float rotationSpeed = 100f;

    [Header("Sensor Settings")]
    public float raycastDistance = 1.5f;
    public LayerMask obstacleMask;

    private Vector3 currentDirection;
    private bool isTurning = false; 

    void Start()
    {
        currentDirection = transform.forward;
    }

    void Update()
    {
        if (!isTurning)
        {
            MoveForward();
            CheckForObstacles();
        }
    }

    void MoveForward()
    {
        transform.Translate(currentDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    void CheckForObstacles()
    {
        bool obstacleDetected = false;

        if (Physics.Raycast(transform.position, currentDirection, raycastDistance, obstacleMask))
        {
            Debug.DrawRay(transform.position, currentDirection * raycastDistance, Color.red);
            obstacleDetected = true;
        }
        else
        {
            Debug.DrawRay(transform.position, currentDirection * raycastDistance, Color.green);
        }

        Vector3 leftDirection = Quaternion.Euler(0, -45, 0) * currentDirection;
        if (Physics.Raycast(transform.position, leftDirection, raycastDistance * 0.7f, obstacleMask))
        {
            Debug.DrawRay(transform.position, leftDirection * raycastDistance * 0.7f, Color.yellow);
        }

        Vector3 rightDirection = Quaternion.Euler(0, 45, 0) * currentDirection;
        if (Physics.Raycast(transform.position, rightDirection, raycastDistance * 0.7f, obstacleMask))
        {
            Debug.DrawRay(transform.position, rightDirection * raycastDistance * 0.7f, Color.yellow);
        }

        if (obstacleDetected)
        {
            AvoidObstacle();
        }
    }

    void AvoidObstacle()
    {
        StartCoroutine(TurnCoroutine());
    }

    IEnumerator TurnCoroutine()
    {
        isTurning = true;
        float turnAngle = Random.Range(90f, 135f) * (Random.Range(0, 2) == 0 ? 1 : -1);
        float targetRotation = transform.eulerAngles.y + turnAngle;

        float currentRotation = transform.eulerAngles.y;
        float rotationProgress = 0f;

        while (rotationProgress < 1f)
        {
            rotationProgress += rotationSpeed * Time.deltaTime / Mathf.Abs(turnAngle);
            float newRotation = Mathf.LerpAngle(currentRotation, targetRotation, rotationProgress);
            transform.rotation = Quaternion.Euler(0, newRotation, 0);
            yield return null;
        }
        currentDirection = transform.forward;
        isTurning = false;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, currentDirection * raycastDistance);

        Gizmos.color = Color.cyan;
        Vector3 leftDir = Quaternion.Euler(0, -45, 0) * currentDirection;
        Vector3 rightDir = Quaternion.Euler(0, 45, 0) * currentDirection;
        Gizmos.DrawRay(transform.position, leftDir * raycastDistance * 0.7f);
        Gizmos.DrawRay(transform.position, rightDir * raycastDistance * 0.7f);
    }
}