using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Zenject;
using Interface;


namespace MiniMap
{

    public sealed class RadarObj : MonoBehaviour
    {
        [SerializeField] private Image _ico;
        private IRadar _radar;

        [Inject]
        private void Inject (IRadar radar) => _radar = radar;

        private void Awake()
        {
            this.OnEnableAsObservable().Subscribe(_ => _radar.RegisterRadarObject(gameObject, _ico)).AddTo(this);
            this.OnDisableAsObservable().Subscribe(_ => _radar.RemoveRadarObject(gameObject)).AddTo(this);
            _radar.RegisterRadarObject(gameObject, _ico);
        }

        private void OnValidate() => _ico = Resources.Load<Image>(path: "MiniMap/RadarObject");

    }
}
