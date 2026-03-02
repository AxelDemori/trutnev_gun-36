using UnityEngine;

namespace InteractiveObjects
{ 
    public abstract class InteractiveObject : MonoBehaviour
    {
        private bool _isInteractable;

        protected bool IsInteractable
        {
            get => _isInteractable;
            private set
            {
                _isInteractable = value;
                GetComponent<Renderer>().enabled = _isInteractable;
                GetComponent<Collider>().enabled = _isInteractable;
            }
        }

        private void OnTriggerEnter(Collider other)
        { 
            if (!IsInteractable || !other.CompareTag("Player"))
            { 
                return; 
            }
            Interaction(other.gameObject);
            IsInteractable = false;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!IsInteractable || !other.gameObject.CompareTag("Player"))
            {
                return;
            }
            Interaction(other.gameObject);
            IsInteractable = false;
        }

        protected abstract void Interaction(GameObject otherGameObject);

        public abstract void Execute();

        private void Start()
        {
            IsInteractable = true;
        }

        private void Update()

        {
            Execute();
        }
    }
}
