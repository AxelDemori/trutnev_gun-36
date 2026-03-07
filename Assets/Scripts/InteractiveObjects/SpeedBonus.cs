using Interface;
using System.Collections;
using System.Model;
using UnityEngine;
using Zenject;

namespace InteractiveObjects
{
    public sealed class SpeedBonus : InteractiveObject
    {
        private Material _material;
        private PlayerSpeed _playerSpeed;
        private float _speedMultiplier = 2f;
        private GameObject _player;

        private void Awake()
        {
            _material = GetComponent<Renderer>().material;
            _material.color = Color.blue;
            transform.localScale = new Vector3(1.5f, 0.2f, 5f);

            Collider collider = GetComponent<Collider>();
            if (collider == null)
            {
                gameObject.AddComponent<BoxCollider>();
            }
        }

        [Inject]
        private void Inject(PlayerSpeed playerSpeed)
        {
            _playerSpeed = playerSpeed;
        }

        protected override void Interaction(GameObject otherGameObject)
        {
            _player = otherGameObject;

            Debug.LogError(message: "SPEED ACTIVATED!");

            _player.GetComponent<MonoBehaviour>().StartCoroutine(SpeedBoostCoroutine());
            gameObject.SetActive(false);
            Destroy(gameObject, 0.5f);
        }

        private IEnumerator SpeedBoostCoroutine()
        {
            if (_playerSpeed == null) yield break;

            float originalSpeed = _playerSpeed.Speed.Value;
            float boostedSpeed = originalSpeed * _speedMultiplier;

            _playerSpeed.Speed.Value = boostedSpeed;

            yield return new WaitForSeconds(1f);

            _playerSpeed.Speed.Value = originalSpeed;
        }

        public override void Execute() { }
    }
}