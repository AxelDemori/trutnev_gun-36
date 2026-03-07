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
        private IRadar _radar;
        private Image _ico;

        [Inject]
        private void Inject(IRadar radar) => _radar = radar;

        private void Awake()
        {
            if (_radar == null) return;

            _ico = CreateIcon();
            this.OnEnableAsObservable().Subscribe(_ => _radar.RegisterRadarObject(gameObject, _ico)).AddTo(this);
            this.OnDisableAsObservable().Subscribe(_ => _radar.RemoveRadarObject(gameObject)).AddTo(this);

            if (enabled) _radar.RegisterRadarObject(gameObject, _ico);
        }

        private Image CreateIcon()
        {
            var iconObj = new GameObject($"Icon_{gameObject.name}");
            var icon = iconObj.AddComponent<Image>();

            icon.color = gameObject.CompareTag("Player") ? Color.green : Color.white;
            icon.rectTransform.sizeDelta = new Vector2(10, 10);

            return icon;
        }

        private void OnDestroy()
        {
            if (_ico != null) Destroy(_ico.gameObject);
        }
    }
}
