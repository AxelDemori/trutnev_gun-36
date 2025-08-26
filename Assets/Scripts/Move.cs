using UnityEngine;

public class Move : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
      
        float moveX = Input.GetAxis("Horizontal"); 
        float moveY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, moveY, 0) * moveSpeed * Time.deltaTime;

        transform.Translate(movement);
    }
}