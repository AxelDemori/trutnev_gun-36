using UnityEngine;

namespace Sounds
{
    public class SoundSpeed : MonoBehaviour
    {
        [SerializeField] private AudioClip _sound;
        private bool _hasPlayed = false;

        private void OnTriggerEnter(Collider other)
        {
            if (_hasPlayed) return;

            if (other.CompareTag("Player") && _sound != null)
            {
                AudioSource.PlayClipAtPoint(_sound, transform.position);
                _hasPlayed = true;
            }
        }
    }
}