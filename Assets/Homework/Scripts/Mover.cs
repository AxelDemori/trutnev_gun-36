using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
    [SerializeField] private Vector3 _start;
    [SerializeField] private Vector3 _end;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _delay = 1f;

    private Rigidbody _rigidbody;
    private bool _movingToEnd = true;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = true;
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        }

        StartCoroutine(MoveObject());
    }

    private System.Collections.IEnumerator MoveObject()
    {
        while (true)
        {
            Vector3 target = _movingToEnd ? _end : _start;

            while (Vector3.Distance(_rigidbody.position, target) > 0.01f)
            {
                if (_rigidbody != null)
                {
                    Vector3 direction = (target - _rigidbody.position).normalized;
                    Vector3 movement = direction * _speed * Time.fixedDeltaTime;

                    _rigidbody.MovePosition(Vector3.MoveTowards(
                        _rigidbody.position,
                        target,
                        _speed * Time.fixedDeltaTime
                    ));
                }

                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForSeconds(_delay);
            _movingToEnd = !_movingToEnd;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_start, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_end, 0.1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_start, _end);
    }
}