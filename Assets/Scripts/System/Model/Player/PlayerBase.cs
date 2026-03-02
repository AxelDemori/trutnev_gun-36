using UnityEngine;

namespace System.Model
{
    public abstract class PlayerBase : MonoBehaviour
    {
        public float Speed = 3.0f;
        protected abstract void Move(Vector3 direction);

    }
}
