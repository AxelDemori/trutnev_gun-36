using System.Model;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace System.Presenter
{
    public sealed class UIPresenter : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_bonusCounter;
        [SerializeField] private GameObject m_gameOverPanel;
        private BonusCount _bonusCounter;
        private PlayerHealth _health;

        [Inject]
        private void Inject(BonusCount bonusCount, PlayerHealth health)
        {
            _bonusCounter = bonusCount;
            _health = health;

            _bonusCounter.Count
                .Subscribe(value => m_bonusCounter.text = value.ToString())
                .AddTo(this);

            _health.Health
                .Subscribe(healthValue => m_gameOverPanel.SetActive(healthValue <= 0))
                .AddTo(this);
        }
    }
}
