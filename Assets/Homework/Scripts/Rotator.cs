using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 _rotate;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        StartCoroutine(RotateObject());
    }

    private System.Collections.IEnumerator RotateObject()
    {
        while (true)
        {
            Quaternion deltaRotation = Quaternion.Euler(_rotate * Time.fixedDeltaTime);
            _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);

            yield return new WaitForFixedUpdate();
        }
    }

    public void SetRotation(Vector3 rotation)
    {
        _rotate = rotation;
    }
}