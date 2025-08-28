using UnityEngine;

public class Gates : MonoBehaviour
{
    private int _score;

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            _score++;
            Destroy(other.gameObject);
            Debug.Log($"—чет: {_score}");
        }
    }
}