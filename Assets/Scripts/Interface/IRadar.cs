using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Interface
{
    public interface IRadar
    {
        void RegisterRadarObject(GameObject owner, Image icon);
        void RemoveRadarObject(GameObject owner);
    }
}
