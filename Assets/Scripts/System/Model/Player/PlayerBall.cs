using System.CodeDom;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace System.Model
{ 
    public sealed class PlayerBall : PlayerBase
    {
        [SerializeField] private Rigidbody _rigidbody;
        [Inject] private IAxisInput _input;
        private IDisposable _disposable;

        [Inject]
        private void Inject(IAxisInput input)
        {
            _input = input;
            _disposable = _input.AxisInput.Subscribe(Move);
        }

        private void Awake()
        {
            this.OnDisableAsObservable()
                .Subscribe(_ => _disposable.Dispose())
                .AddTo(this);

            this.OnEnableAsObservable()
                .Subscribe(_ => _disposable = _input.AxisInput.Subscribe(Move))
                .AddTo(this);
        }

        protected override void Move(Vector3 direction) => _rigidbody.AddForce(direction * Speed);

    }

}
