using UnityEngine;
using UnityEngine.UI;

namespace MiniMap
{

    public sealed class RadarObj : MonoBehaviour
    {
        [SerializeField] private Image _ico;

        private void OnValidate() => _ico = Resources.Load<Image>(path: "MiniMap/RadarObject");

        private void OnDisable() => Radar.RemoveRadarObject(gameObject);

        private void OnEnable() => Radar.RegisterRadarObject(gameObject, _ico);
    }
}
