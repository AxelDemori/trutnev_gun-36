using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace InteractiveObjects
{
    public abstract class InteractiveObject : MonoBehaviour
    {
        private bool _isInteractable;
        private Renderer _renderer;
        private Collider _collider;

        protected bool IsInteractable
        {
            get => _isInteractable;
            private set
            {
                _isInteractable = value;
                if (_renderer != null) _renderer.enabled = _isInteractable;
                if (_collider != null) _collider.enabled = _isInteractable;
            }
        }

        protected abstract void Interaction(GameObject otherGameObject);
        public abstract void Execute();

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _collider = GetComponent<Collider>();
        }

        private void Start()
        {
            IsInteractable = true;

            this.OnCollisionEnterAsObservable()
                .Where(collision => IsInteractable && collision.gameObject.CompareTag("Player"))
                .Subscribe(collision => Interaction(collision.gameObject))
                .AddTo(this);

            this.OnTriggerEnterAsObservable()
                .Where(collider => IsInteractable && collider.CompareTag("Player"))
                .Do(_ => IsInteractable = false)
                .Subscribe(collider => Interaction(collider.gameObject))
                .AddTo(this);

            Observable.EveryUpdate()
                .Subscribe(_ => Execute())
                .AddTo(this);
        }
    }
}