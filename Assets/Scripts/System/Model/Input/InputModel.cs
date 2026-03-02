using UniRx;
using UnityEngine;

namespace System.Model
{
    public sealed class InputModel : IVectorSet, IAxisInput
    {
        private ReactiveProperty<Vector3> _axisInput;
        public IObservable<Vector3> AxisInput => _axisInput;
        public InputModel() => _axisInput = new ReactiveProperty<Vector3>();
        public void SetVector(Vector3 vector3) => _axisInput.SetValueAndForceNotify(vector3);
    }
}
