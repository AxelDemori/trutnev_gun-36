using Unity.VisualScripting;
using UnityEngine;

namespace InteractiveObjects
{
    public sealed class DamageObject : InteractiveObject
    {
        protected override void Interaction(GameObject otherGameObject)
        {
            Debug.LogError(message: "Damage applied");
        }

        public override void Execute()
        {

        }
    }
}