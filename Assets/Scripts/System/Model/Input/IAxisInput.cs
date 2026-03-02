using UnityEngine;

namespace System.Model
{
    public interface IAxisInput
    {
        IObservable<Vector3> AxisInput { get; }

    }
}