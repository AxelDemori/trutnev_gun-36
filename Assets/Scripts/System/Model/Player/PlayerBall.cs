using UniRx;
using UnityEngine;
using Zenject;

namespace System.Model
{ 
    public sealed class PlayerBall : PlayerBase
    {
        [SerializeField] private Rigidbody _rigidbody;
        [Inject] private IAxisInput _input;
        private IDisposable _disposable;

        private void OnEnable() => _disposable = _input.AxisInput.Subscribe(Move);

        private void OnDisable() => _disposable.Dispose();

        protected override void Move(Vector3 direction) => _rigidbody.AddForce(direction * Speed);

    }

}
