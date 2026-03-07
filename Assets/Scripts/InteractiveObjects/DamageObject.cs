using System.Model;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace InteractiveObjects
{
    public sealed class DamageObject : InteractiveObject
    {
        private PlayerHealth _health;

        [Inject]
        private void Inject(PlayerHealth health) => _health = health;
        protected override void Interaction(GameObject otherGameObject)
        {
            _health.Health.Value -= 70;
        }

        public override void Execute()
        {

        }
    }
}