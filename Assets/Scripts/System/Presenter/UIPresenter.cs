using System.Diagnostics;
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
        [SerializeField] private TMP_Text m_hpText;
        [SerializeField] private GameObject m_gameOverPanel;

        private BonusCount _bonusCounter;
        private PlayerHealth _health;

        [Inject]
        private void Inject(BonusCount bonusCount, PlayerHealth health)
        {
            _bonusCounter = bonusCount;
            _health = health;
            if (m_bonusCounter != null)
            {
                _bonusCounter.Count
                    .Subscribe(value => m_bonusCounter.text = value.ToString())
                    .AddTo(this);
            }

            if (m_hpText != null && m_gameOverPanel != null)
            {
                _health.Health
                    .Subscribe(healthValue => {

                        m_hpText.text = $"HP: {healthValue}";
                        if (healthValue <= 0)
                        {
                            ShowGameOver();
                        }
                    })
                    .AddTo(this);
            }
        }

        private void ShowGameOver()
        {
            m_gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f;
        }
    }
}