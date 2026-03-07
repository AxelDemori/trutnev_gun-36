using Interface;
using System.Model;
using UnityEngine;
using Zenject;
using static UnityEngine.Random;

namespace InteractiveObjects
{
    public sealed class GoodBonus : InteractiveObject, IFlay
    {
        private Material _material;
        private float _lengthFlay;
        private PlayerHealth _health;
        private BonusCount _counter;
      
        private void Awake()
        {
            _material = GetComponent<Renderer>().material;
            _material.color = Color.green;
            _lengthFlay = Range(1.0f, 0.5f);
        }

        [Inject]
        private void Inject(PlayerHealth health, BonusCount count)
        { 
            _health = health;
            _counter = count;
        }

        protected override void Interaction(GameObject otherGameObject)
        {
            _health.Health.SetValueAndForceNotify(20);
            _counter.Count.Value += 1;
        }

        public override void Execute()
        {
            if (!IsInteractable) { return; }
            Flay();
        }

        public void Flay()

        {
            transform.localPosition = new Vector3(transform.localPosition.x,
            y:Mathf.PingPong(t:Time.time, _lengthFlay),
            transform.localPosition.z);
        }
    }
}

