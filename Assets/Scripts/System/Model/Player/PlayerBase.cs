using UniRx;
using UnityEngine;
using UnityEngine.Windows;
using Zenject;

namespace System.Model
{
    public abstract class PlayerBase : MonoBehaviour
    {
        public float Speed = 3.0f;
        protected abstract void Move(Vector3 direction);

        [Inject]
        private void Inject(PlayerSpeed speed)
        {
            SpeedModel = speed;
        }

        protected PlayerSpeed SpeedModel { get; set; }

    }
}
