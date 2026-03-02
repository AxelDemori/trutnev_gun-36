using Interface;
using UnityEngine;
using static UnityEngine.Random;

namespace InteractiveObjects
{
    public sealed class GoodBonus : InteractiveObject, IFlay
    {
        private Material _material;
        private float _lengthFlay;

        private void Awake()
        {
            _material = GetComponent<Renderer>().material;
            _material.color = Color.green;
            _lengthFlay = Range(1.0f, 0.5f);
        }

        protected override void Interaction(GameObject otherGameObject)
        {
            Debug.LogError(message:"HP RESTORED!");
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

